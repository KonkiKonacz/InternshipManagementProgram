using Microsoft.EntityFrameworkCore;

namespace InternshipManagementProgram.Tests;

/// <summary>
/// Testy widokow bazodanowych. Sprawdzaja, ze kazdy widok jest poprawnie
/// zmapowany w EF Core i wykonuje sie bez bledu, a kluczowy V_PraktykiPelne
/// zwraca spojne dane.
/// </summary>
[TestClass]
public class ViewTests : DatabaseTestBase
{
    [TestMethod]
    public async Task VPraktykiPelne_ZwracaWierszeZDanymiStudentaIFirmy()
    {
        await using var db = CreateContext();
        var rows = await db.VPraktykiPelnes.ToListAsync();

        Assert.IsNotEmpty(rows, "Widok V_PraktykiPelne nie zwrocil zadnych praktyk.");
        Assert.IsTrue(rows.All(r => !string.IsNullOrEmpty(r.NrAlbumu)), "Kazdy wiersz powinien miec NrAlbumu.");
        Assert.IsTrue(rows.All(r => !string.IsNullOrEmpty(r.NazwaFirmy)), "Kazdy wiersz powinien miec nazwe firmy.");
        Assert.IsTrue(rows.All(r => !string.IsNullOrEmpty(r.Status)), "Kazdy wiersz powinien miec status.");
    }

    [TestMethod]
    public async Task VPraktykiPelne_LiczbaWierszyZgodnaZTabelaPraktyka()
    {
        // Widok laczy Praktyka z wymiarami (INNER JOIN) - powinien pokryc wszystkie praktyki.
        await using var db = CreateContext();
        var widok = await db.VPraktykiPelnes.CountAsync();
        var tabela = await db.Praktykas.CountAsync();
        Assert.AreEqual(tabela, widok, "Widok V_PraktykiPelne powinien pokrywac wszystkie praktyki.");
    }

    [TestMethod]
    public async Task VStudenciBezPraktyk_WykonujeSieBezBledu()
    {
        await using var db = CreateContext();
        var rows = await db.VStudenciBezPraktyks.ToListAsync();
        Assert.IsNotNull(rows);
    }

    [TestMethod]
    public async Task VStudenciNiezaliczeni_WykonujeSieBezBledu()
    {
        await using var db = CreateContext();
        var rows = await db.VStudenciNiezaliczenis.ToListAsync();
        Assert.IsNotNull(rows);
    }

    [TestMethod]
    public async Task VPraktykiStatystyki_WykonujeSieBezBledu()
    {
        await using var db = CreateContext();
        var rows = await db.VPraktykiStatystykis.ToListAsync();
        Assert.IsNotNull(rows);
    }

    [TestMethod]
    public async Task VStatystykiKierunkow_WykonujeSieBezBledu()
    {
        await using var db = CreateContext();
        var rows = await db.VStatystykiKierunkows.ToListAsync();
        Assert.IsNotNull(rows);
    }
}
