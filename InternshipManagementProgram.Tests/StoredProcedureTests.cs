using System.Data;
using InternshipManagementProgram.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace InternshipManagementProgram.Tests;

/// <summary>
/// Testy procedur skladowanych uzywanych przez aplikacje: P_ZgloszeniePraktyki,
/// P_ZmianaStatusu, P_WystawienieOceny. Kazdy test działa w transakcji wycofywanej
/// na koncu, wiec realne dane w bazie pozostaja nienaruszone. Sprawdzamy zarowno
/// sciezki poprawne, jak i walidacje wymuszane przez triggery.
/// </summary>
[TestClass]
public class StoredProcedureTests : DatabaseTestBase
{
    // Pomocnik: zglasza testowa praktyke przez procedure i zwraca jej IDPraktyki.
    private static async Task<int> ZglosTestowaPraktykeAsync(
        PraktykiStudenckieContext db, string rodzaj, DateOnly? rozp = null, DateOnly? zak = null)
    {
        var firmaId = await db.Firmas.Select(f => f.Idfirmy).FirstAsync();
        var opiekunId = await db.Opiekuns.Select(o => o.Idopiekuna).FirstAsync();

        // Parametry opcjonalne moga byc NULL - EF nie mapuje surowego DBNull,
        // wiec budujemy SqlParameter z jawnym typem kolumny.
        await db.Database.ExecuteSqlRawAsync(
            "EXEC P_ZgloszeniePraktyki @NrAlbumu, @IDFirmy, @IDOpiekuna, @RodzajPraktyki, @DataRozpoczecia, @DataZakonczenia, @IDOsoby",
            new SqlParameter("@NrAlbumu", TestowyNrAlbumu),
            new SqlParameter("@IDFirmy", firmaId),
            new SqlParameter("@IDOpiekuna", opiekunId),
            new SqlParameter("@RodzajPraktyki", rodzaj),
            new SqlParameter("@DataRozpoczecia", SqlDbType.Date) { Value = (object?)rozp ?? DBNull.Value },
            new SqlParameter("@DataZakonczenia", SqlDbType.Date) { Value = (object?)zak ?? DBNull.Value },
            new SqlParameter("@IDOsoby", SqlDbType.Int) { Value = DBNull.Value });

        return await db.Praktykas.AsNoTracking()
            .Where(p => p.NrAlbumu == TestowyNrAlbumu && p.RodzajPraktyki == rodzaj)
            .Select(p => p.Idpraktyki)
            .SingleAsync();
    }

    // ---------- P_ZgloszeniePraktyki ----------

    [TestMethod]
    public async Task P_ZgloszeniePraktyki_WstawiaNowaPraktykeZeStatusemZgloszona()
    {
        var rodzaj = UnikalnyRodzaj();
        await using var db = CreateContext();
        await using var tx = await db.Database.BeginTransactionAsync();
        try
        {
            var przed = await db.Praktykas.CountAsync(p => p.NrAlbumu == TestowyNrAlbumu);
            var id = await ZglosTestowaPraktykeAsync(db, rodzaj);
            var po = await db.Praktykas.CountAsync(p => p.NrAlbumu == TestowyNrAlbumu);

            Assert.AreEqual(przed + 1, po, "Powinna powstac dokladnie jedna nowa praktyka.");

            var nowa = await db.Praktykas.AsNoTracking().SingleAsync(p => p.Idpraktyki == id);
            Assert.AreEqual(rodzaj, nowa.RodzajPraktyki);
            Assert.AreEqual("Zgłoszona", nowa.Status, "Nowa praktyka powinna miec domyslny status Zgloszona.");
        }
        finally
        {
            await SafeRollbackAsync(tx);
        }
    }

    [TestMethod]
    public async Task P_ZgloszeniePraktyki_NieistniejacyStudent_RzucaWyjatek()
    {
        await using var db = CreateContext();
        await using var tx = await db.Database.BeginTransactionAsync();
        try
        {
            var firmaId = await db.Firmas.Select(f => f.Idfirmy).FirstAsync();
            var opiekunId = await db.Opiekuns.Select(o => o.Idopiekuna).FirstAsync();
            try
            {
                await db.Database.ExecuteSqlRawAsync(
                    "EXEC P_ZgloszeniePraktyki @NrAlbumu={0}, @IDFirmy={1}, @IDOpiekuna={2}, @RodzajPraktyki={3}",
                    "00000", firmaId, opiekunId, UnikalnyRodzaj());
                Assert.Fail("Oczekiwano SqlException dla nieistniejacego studenta.");
            }
            catch (SqlException ex)
            {
                StringAssert.Contains(WszystkieKomunikaty(ex), "nie istnieje");
            }
        }
        finally
        {
            await SafeRollbackAsync(tx);
        }
    }

    [TestMethod]
    public async Task P_ZgloszeniePraktyki_NieistniejacaFirma_RzucaWyjatek()
    {
        await using var db = CreateContext();
        await using var tx = await db.Database.BeginTransactionAsync();
        try
        {
            var opiekunId = await db.Opiekuns.Select(o => o.Idopiekuna).FirstAsync();
            try
            {
                await db.Database.ExecuteSqlRawAsync(
                    "EXEC P_ZgloszeniePraktyki @NrAlbumu={0}, @IDFirmy={1}, @IDOpiekuna={2}, @RodzajPraktyki={3}",
                    TestowyNrAlbumu, -1, opiekunId, UnikalnyRodzaj());
                Assert.Fail("Oczekiwano SqlException dla nieistniejacej firmy.");
            }
            catch (SqlException ex)
            {
                StringAssert.Contains(WszystkieKomunikaty(ex), "nie istnieje");
            }
        }
        finally
        {
            await SafeRollbackAsync(tx);
        }
    }

    // ---------- P_ZmianaStatusu ----------

    [TestMethod]
    public async Task P_ZmianaStatusu_ZgloszonaNaZatwierdzona_UstawiaStatusIDate()
    {
        var rodzaj = UnikalnyRodzaj();
        var dataRozp = new DateOnly(2099, 1, 10);
        await using var db = CreateContext();
        await using var tx = await db.Database.BeginTransactionAsync();
        try
        {
            var id = await ZglosTestowaPraktykeAsync(db, rodzaj);

            await db.Database.ExecuteSqlRawAsync(
                "EXEC P_ZmianaStatusu @IDPraktyki={0}, @NowyStatus={1}, @DataRozpoczecia={2}",
                id, "Zatwierdzona", dataRozp);

            var p = await db.Praktykas.AsNoTracking().SingleAsync(x => x.Idpraktyki == id);
            Assert.AreEqual("Zatwierdzona", p.Status);
            Assert.AreEqual(dataRozp, p.DataRozpoczecia, "Procedura powinna zapisac przekazana date rozpoczecia.");
        }
        finally
        {
            await SafeRollbackAsync(tx);
        }
    }

    [TestMethod]
    public async Task P_ZmianaStatusu_ZatwierdzenieBezDaty_RzucaWyjatek()
    {
        // TR_Praktyka_DataPrzyZatwierdzeniu: przejscie Zgloszona->Zatwierdzona wymaga daty rozpoczecia.
        var rodzaj = UnikalnyRodzaj();
        await using var db = CreateContext();
        await using var tx = await db.Database.BeginTransactionAsync();
        try
        {
            var id = await ZglosTestowaPraktykeAsync(db, rodzaj);
            try
            {
                await db.Database.ExecuteSqlRawAsync(
                    "EXEC P_ZmianaStatusu @IDPraktyki={0}, @NowyStatus={1}",
                    id, "Zatwierdzona");
                Assert.Fail("Oczekiwano SqlException - zatwierdzenie bez daty rozpoczecia.");
            }
            catch (SqlException ex)
            {
                StringAssert.Contains(WszystkieKomunikaty(ex), "daty rozpocz");
            }
        }
        finally
        {
            await SafeRollbackAsync(tx);
        }
    }

    [TestMethod]
    public async Task P_ZmianaStatusu_NiedozwolonePrzejscie_RzucaWyjatek()
    {
        // TR_Praktyka_KontrolaStatusu: nie wolno przeskoczyc ze Zgloszona od razu na Zakonczona.
        var rodzaj = UnikalnyRodzaj();
        await using var db = CreateContext();
        await using var tx = await db.Database.BeginTransactionAsync();
        try
        {
            var id = await ZglosTestowaPraktykeAsync(db, rodzaj);
            try
            {
                await db.Database.ExecuteSqlRawAsync(
                    "EXEC P_ZmianaStatusu @IDPraktyki={0}, @NowyStatus={1}",
                    id, "Zakończona");
                Assert.Fail("Oczekiwano SqlException - niedozwolone przejscie statusu.");
            }
            catch (SqlException ex)
            {
                StringAssert.Contains(WszystkieKomunikaty(ex), "Niedozwolona zmiana statusu");
            }
        }
        finally
        {
            await SafeRollbackAsync(tx);
        }
    }

    // ---------- P_WystawienieOceny ----------

    [TestMethod]
    public async Task P_WystawienieOceny_PelnyCykl_UstawiaOceneIStatusZaliczona()
    {
        // Przeprowadzamy praktyke przez caly dozwolony cykl statusow az do oceny.
        var rodzaj = UnikalnyRodzaj();
        await using var db = CreateContext();
        await using var tx = await db.Database.BeginTransactionAsync();
        try
        {
            var id = await ZglosTestowaPraktykeAsync(db, rodzaj);

            await db.Database.ExecuteSqlRawAsync(
                "EXEC P_ZmianaStatusu @IDPraktyki={0}, @NowyStatus={1}, @DataRozpoczecia={2}",
                id, "Zatwierdzona", new DateOnly(2099, 1, 10));
            await db.Database.ExecuteSqlRawAsync(
                "EXEC P_ZmianaStatusu @IDPraktyki={0}, @NowyStatus={1}", id, "W trakcie");
            await db.Database.ExecuteSqlRawAsync(
                "EXEC P_ZmianaStatusu @IDPraktyki={0}, @NowyStatus={1}", id, "Zakończona");

            await db.Database.ExecuteSqlRawAsync(
                "EXEC P_WystawienieOceny @IDPraktyki={0}, @Ocena={1}", id, 4.5m);

            var p = await db.Praktykas.AsNoTracking().SingleAsync(x => x.Idpraktyki == id);
            Assert.AreEqual(4.5m, p.Ocena);
            // TR_Praktyka_OcenaStatus: ocena > 2.0 automatycznie ustawia status Zaliczona.
            Assert.AreEqual("Zaliczona", p.Status);
        }
        finally
        {
            await SafeRollbackAsync(tx);
        }
    }

    [TestMethod]
    public async Task P_WystawienieOceny_OcenaNiedostatecznaUstawiaNiezaliczona()
    {
        var rodzaj = UnikalnyRodzaj();
        await using var db = CreateContext();
        await using var tx = await db.Database.BeginTransactionAsync();
        try
        {
            var id = await ZglosTestowaPraktykeAsync(db, rodzaj);

            await db.Database.ExecuteSqlRawAsync(
                "EXEC P_ZmianaStatusu @IDPraktyki={0}, @NowyStatus={1}, @DataRozpoczecia={2}",
                id, "Zatwierdzona", new DateOnly(2099, 1, 10));
            await db.Database.ExecuteSqlRawAsync(
                "EXEC P_ZmianaStatusu @IDPraktyki={0}, @NowyStatus={1}", id, "W trakcie");
            await db.Database.ExecuteSqlRawAsync(
                "EXEC P_ZmianaStatusu @IDPraktyki={0}, @NowyStatus={1}", id, "Zakończona");

            await db.Database.ExecuteSqlRawAsync(
                "EXEC P_WystawienieOceny @IDPraktyki={0}, @Ocena={1}", id, 2.0m);

            var p = await db.Praktykas.AsNoTracking().SingleAsync(x => x.Idpraktyki == id);
            Assert.AreEqual(2.0m, p.Ocena);
            Assert.AreEqual("Niezaliczona", p.Status);
        }
        finally
        {
            await SafeRollbackAsync(tx);
        }
    }

    [TestMethod]
    public async Task P_WystawienieOceny_OcenaPozaZakresem_RzucaWyjatek()
    {
        // Procedura dopuszcza tylko 2.0, 3.0, 3.5, 4.0, 4.5, 5.0.
        await using var db = CreateContext();
        await using var tx = await db.Database.BeginTransactionAsync();
        try
        {
            var istniejaca = await db.Praktykas.Select(p => p.Idpraktyki).FirstAsync();
            try
            {
                await db.Database.ExecuteSqlRawAsync(
                    "EXEC P_WystawienieOceny @IDPraktyki={0}, @Ocena={1}", istniejaca, 2.5m);
                Assert.Fail("Oczekiwano SqlException - ocena 2.5 jest niedozwolona.");
            }
            catch (SqlException ex)
            {
                StringAssert.Contains(WszystkieKomunikaty(ex), "niedozwolona");
            }
        }
        finally
        {
            await SafeRollbackAsync(tx);
        }
    }

    [TestMethod]
    public async Task P_WystawienieOceny_PrzedZakonczeniem_RzucaWyjatek()
    {
        // TR_Praktyka_OcenaStatus: ocene mozna wystawic tylko dla praktyki o statusie Zakonczona.
        var rodzaj = UnikalnyRodzaj();
        await using var db = CreateContext();
        await using var tx = await db.Database.BeginTransactionAsync();
        try
        {
            var id = await ZglosTestowaPraktykeAsync(db, rodzaj); // status Zgloszona
            try
            {
                await db.Database.ExecuteSqlRawAsync(
                    "EXEC P_WystawienieOceny @IDPraktyki={0}, @Ocena={1}", id, 4.0m);
                Assert.Fail("Oczekiwano SqlException - ocena przed zakonczeniem praktyki.");
            }
            catch (SqlException ex)
            {
                StringAssert.Contains(WszystkieKomunikaty(ex), "wystawiona tylko");
            }
        }
        finally
        {
            await SafeRollbackAsync(tx);
        }
    }
}
