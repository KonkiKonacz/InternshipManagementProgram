using InternshipManagementProgram.Components;
using InternshipManagementProgram.Models;
using InternshipManagementProgram.Services;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using QuestPDF.Infrastructure;

QuestPDF.Settings.License = LicenseType.Community;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();

builder.Services.AddDbContextFactory<PraktykiStudenckieContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("PraktykiStudenckie")));

builder.Services.AddScoped<PraktykiStudenckieContext>(sp =>
    sp.GetRequiredService<IDbContextFactory<PraktykiStudenckieContext>>().CreateDbContext());

builder.Services.AddScoped<AppState>();
builder.Services.AddScoped<CertificateService>();

var app = builder.Build();

// Rozgrzewka EF Core w tle: pierwsze utworzenie kontekstu buduje model i otwiera
// polaczenie (2-3s) - robimy to przy starcie, by panel po zalogowaniu wczytywal sie od razu.
_ = Task.Run(async () =>
{
    try
    {
        var factory = app.Services.GetRequiredService<IDbContextFactory<PraktykiStudenckieContext>>();
        await using var db = await factory.CreateDbContextAsync();
        await db.VPraktykiPelnes.AsNoTracking().Take(1).ToListAsync();
    }
    catch { /* rozgrzewka best-effort - nie blokuje startu aplikacji */ }
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
