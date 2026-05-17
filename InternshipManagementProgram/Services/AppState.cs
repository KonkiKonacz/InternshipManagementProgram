namespace InternshipManagementProgram.Services;

public class AppState
{
    public string? Username { get; private set; }
    public string? Role { get; private set; }
    public string? NrAlbumu { get; private set; }

    public bool IsLoggedIn => Role is not null;
    public bool IsAdmin => Role == "admin";
    public bool IsStudent => Role == "student";

    public event Action? Changed;

    public void LoginAsStudent()
    {
        Username = "student";
        Role = "student";
        NrAlbumu = "62753";
        Changed?.Invoke();
    }

    public void LoginAsAdmin()
    {
        Username = "admin";
        Role = "admin";
        NrAlbumu = null;
        Changed?.Invoke();
    }

    public void Logout()
    {
        Username = null;
        Role = null;
        NrAlbumu = null;
        Changed?.Invoke();
    }
}
