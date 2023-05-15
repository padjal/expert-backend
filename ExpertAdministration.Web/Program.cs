using ExpertAdministration.Web;
using ExpertAdministration.Web.Common;
using ExpertAdministration.Web.Interfaces;
using ExpertAdministration.Web.Services;
using ExpertAdministration.Web.ViewModels;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddHttpClient(Constants.CustomWebApi,
    client => client.BaseAddress = new Uri("https://localhost:7269/"));

builder.Services.AddSingleton<IDatabaseService, DatabaseService>();

builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();

builder.Services.AddScoped<OffersMonitorViewModel>();
builder.Services.AddScoped<OfferReviewViewModel>();

await builder.Build().RunAsync();