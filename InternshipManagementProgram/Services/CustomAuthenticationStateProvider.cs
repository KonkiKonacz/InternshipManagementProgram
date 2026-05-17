using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace InternshipManagementProgram.Services;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private static readonly Dictionary<string, (string Password, string Role, string? NrAlbumu)> Accounts = new()
    {
        ["admin"] = ("admin123", "admin", null),
        ["student"] = ("student123", "student", "62753"),
    };

    private ClaimsPrincipal _currentUser = new(new ClaimsIdentity());

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
        => Task.FromResult(new AuthenticationState(_currentUser));

    public bool TryLogin(string username, string password)
    {
        if (!Accounts.TryGetValue(username, out var account) || account.Password != password)
            return false;

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, username),
            new(ClaimTypes.Role, account.Role),
        };

        if (!string.IsNullOrEmpty(account.NrAlbumu))
            claims.Add(new Claim("NrAlbumu", account.NrAlbumu));

        var identity = new ClaimsIdentity(claims, authenticationType: "Custom");
        _currentUser = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
        return true;
    }

    public void Logout()
    {
        _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
    }
}
