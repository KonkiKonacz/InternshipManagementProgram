using System.Text.Json;
using InternshipManagementProgram.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace InternshipManagementProgram.Tests;

/// <summary>
/// Wspolna baza dla testow czesci bazodanowej. Wczytuje connection string z
/// appsettings.json (podlinkowany z projektu glownego) i udostepnia fabryke
/// kontekstu EF Core. Procedury modyfikujace dane uruchamiamy w transakcji,
/// ktora po asercjach jest wycofywana - prawdziwa baza nie jest zasmiecana.
/// </summary>
public abstract class DatabaseTestBase
{
    // Konto demo studenta - jedyny rekord, na ktorym opiera sie panel studenta.
    protected const string TestowyNrAlbumu = "62753";

    private static readonly string ConnectionString = WczytajConnectionString();

    private static string WczytajConnectionString()
    {
        // appsettings.json jest kopiowany do katalogu wyjsciowego (link w .csproj),
        // dzieki czemu connection string pozostaje wylacznie w projekcie glownym.
        var json = File.ReadAllText("appsettings.json");
        using var doc = JsonDocument.Parse(json);
        return doc.RootElement
            .GetProperty("ConnectionStrings")
            .GetProperty("PraktykiStudenckie")
            .GetString()!;
    }

    /// <summary>Tworzy nowy kontekst EF Core wskazujacy na lokalna baze PraktykiStudenckie.</summary>
    protected static PraktykiStudenckieContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<PraktykiStudenckieContext>()
            .UseSqlServer(ConnectionString)
            .Options;
        return new PraktykiStudenckieContext(options);
    }

    /// <summary>
    /// Wycofuje transakcje bezpiecznie - czesc triggerow sama wykonuje
    /// ROLLBACK TRANSACTION, wiec ponowny rollback rzucilby wyjatek; ignorujemy go.
    /// </summary>
    protected static async Task SafeRollbackAsync(IDbContextTransaction tx)
    {
        try { await tx.RollbackAsync(); }
        catch { /* transakcja juz wycofana przez trigger */ }
    }

    /// <summary>Skleja komunikaty wszystkich bledow z SqlException w jeden tekst do asercji.</summary>
    protected static string WszystkieKomunikaty(SqlException ex)
        => string.Join(" | ", ex.Errors.Cast<SqlError>().Select(e => e.Message));

    /// <summary>Unikalny marker rodzaju praktyki - pozwala odnalezc rekord wstawiony przez test.</summary>
    protected static string UnikalnyRodzaj() => "TEST_" + Guid.NewGuid().ToString("N");
}
