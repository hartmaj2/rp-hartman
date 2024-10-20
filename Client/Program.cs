using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app"); // adds the App.razor component as a root component to the Blazor application (renders it inside the div with #app css selector)
// builder.RootComponents.Add<HeadOutlet>("head::after"); // adding the HeadOutlet seems not necessary (used for dynamically changing title or meta tags in head which i don't need)

// Scoped services live throughout one user connection (a new instance of the service is created only when the page is reloaded)
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<AllergenService>(); // add my allergen service so I can pass list of all possible allergens to components that inject this service
builder.Services.AddScoped<ClientModeService>(); // tracks changes between bug reporting and normal mode
builder.Services.AddScoped<QuestService>(); // track what tasks are active/completed/future 
builder.Services.AddBlazorBootstrap(); // add service for BlazorBootstrap needed for modals and dropdowns

await builder.Build().RunAsync();
