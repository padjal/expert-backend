using ExpertAdministration.Web;
using ExpertAdministration.Web.Interfaces;
using ExpertAdministration.Web.Services;
using ExpertAdministration.Web.ViewModels;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddHttpClient("ExpertAdminWebApi",
    client => client.BaseAddress = new Uri("https://localhost:7269/"));

builder.Services.AddSingleton<IDatabaseService, DatabaseService>();

builder.Services.AddScoped<OffersMonitorViewModel>();

await builder.Build().RunAsync();