using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MusicPlayerApplication.Extensions;
using Radzen;
using Fluxor;
using MusicPlayerApplication;
using MusicPlayerApplication.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Other
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Config
builder.Services.AddConfigurations(builder.Configuration);

// Services
builder.Services.AddServices();

// Http clients
builder.Services.AddHttpClients();

// ViewModel
builder.Services.AddViewModels();

// Radzen
builder.Services.AddRadzenComponents();

// Pattern flux-redux
builder.Services.AddFluxor(options =>
{
    options.ScanAssemblies(typeof(Program).Assembly);
    options.UseReduxDevTools();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(builder.Configuration["YoutubeDl:MusicPath"]),
    RequestPath = builder.Configuration["Song:Path"]
});

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.UseRequestLocalization(options =>
{
    var supportedCultures = new[] { "fr-FR", "en-US" };
    options.SetDefaultCulture(supportedCultures[0])
        .AddSupportedCultures(supportedCultures)
        .AddSupportedUICultures(supportedCultures);
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();