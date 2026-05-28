using InternshipManagementProgram.Models;
using Microsoft.EntityFrameworkCore;

namespace InternshipManagementProgram.Tests;

/// <summary>
/// Testy podstawowego polaczenia z baza PraktykiStudenckie i obecnosci danych,
/// na ktorych opiera sie aplikacja.
/// </summary>
[TestClass]
public class ConnectionTests : DatabaseTestBase
{
    [TestMethod]
    public async Task BazaJestOsiagalna()
    {
        await using var db = CreateContext();
        var canConnect = await db.Database.CanConnectAsync();
        Assert.IsTrue(canConnect, "Nie udalo sie polaczyc z baza PraktykiStudenckie.");
    }

    [TestMethod]
    public async Task NazwaBazyToPraktykiStudenckie()
    {
        await using var db = CreateContext();
        var name = db.Database.GetDbConnection().Database;
        Assert.AreEqual("PraktykiStudenckie", name);
    }

    [TestMethod]
    public async Task TabeleMajaDaneReferencyjne()
    {
        // Mapowanie tabel jest poprawne (zapytanie przechodzi), a aplikacja ma na czym
        // dzialac - formularz zgloszenia wymaga firm i opiekunow, panele wymagaja praktyk.
        await using var db = CreateContext();
        Assert.IsTrue(await db.Wydzials.AnyAsync(), "Brak wydzialow.");
        Assert.IsTrue(await db.Kieruneks.AnyAsync(), "Brak kierunkow.");
        Assert.IsTrue(await db.Students.AnyAsync(), "Brak studentow.");
        Assert.IsTrue(await db.Opiekuns.AnyAsync(), "Brak opiekunow - formularz zgloszenia bylby pusty.");
        Assert.IsTrue(await db.Firmas.AnyAsync(), "Brak firm - formularz zgloszenia bylby pusty.");
        Assert.IsTrue(await db.OsobaKontaktowas.AnyAsync(), "Brak osob kontaktowych.");
        Assert.IsTrue(await db.Praktykas.AnyAsync(), "Brak praktyk.");
    }

    [TestMethod]
    public async Task TestowyStudentIstnieje()
    {
        await using var db = CreateContext();
        var exists = await db.Students.AnyAsync(s => s.NrAlbumu == TestowyNrAlbumu);
        Assert.IsTrue(exists, $"Brak studenta o NrAlbumu {TestowyNrAlbumu} - panel studenta na nim bazuje.");
    }
}
