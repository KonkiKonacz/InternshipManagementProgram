namespace InternshipManagementProgram.Services;

/// <summary>
/// Przechowuje stan zalogowanego uzytkownika (login, rola, nr albumu) w ramach
/// jednego obwodu Blazor. Komponenty subskrybuja event Changed, by odswiezyc widok
/// po zmianie konta. To "udawane" logowanie - nie jest mechanizmem bezpieczenstwa.
/// </summary>
public class AppState
{
    public string? Username { get; private set; }
    public string? Role { get; private set; }
    public string? NrAlbumu { get; private set; }

    public bool IsLoggedIn => Role is not null;
    public bool IsAdmin => Role == "admin";
    public bool IsStudent => Role == "student";

    public event Action? Changed;

    /// <summary>Loguje jako student z numerem albumu testowego konta demo.</summary>
    public void LoginAsStudent()
    {
        Username = "student";
        Role = "student";
        NrAlbumu = "62753";
        Changed?.Invoke();
    }

    /// <summary>Loguje jako administrator (pelnomocnik ds. praktyk) - bez numeru albumu.</summary>
    public void LoginAsAdmin()
    {
        Username = "admin";
        Role = "admin";
        NrAlbumu = null;
        Changed?.Invoke();
    }

    /// <summary>Czysci stan sesji i powiadamia komponenty o wylogowaniu.</summary>
    public void Logout()
    {
        Username = null;
        Role = null;
        NrAlbumu = null;
        Changed?.Invoke();
    }
}
