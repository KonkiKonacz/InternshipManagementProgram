using InternshipManagementProgram.Components;
using InternshipManagementProgram.Models;
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

var app = builder.Build();

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
