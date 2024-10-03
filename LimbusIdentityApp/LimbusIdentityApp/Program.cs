using LimbusIdentityApp.Client.Pages;
using LimbusIdentityApp.Clients;
using LimbusIdentityApp.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

var LimbusIdentityUrl = builder.Configuration["LimbusIdentityUrl"] ??
    throw new Exception("LimbusIdentityUrl is not set");

builder.Services.AddHttpClient<PassiveClient>(
    client => client.BaseAddress = new Uri(LimbusIdentityUrl));

builder.Services.AddHttpClient<SkillClient>(
    client => client.BaseAddress = new Uri(LimbusIdentityUrl));

builder.Services.AddHttpClient<IdentityClient>(
    client => client.BaseAddress = new Uri(LimbusIdentityUrl));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(LimbusIdentityApp.Client._Imports).Assembly);

app.Run();
