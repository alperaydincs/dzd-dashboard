using DZDDashboard.Client.Components;
using DZDDashboard.Client.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
       .AddInteractiveServerComponents();

builder.Services.AddMudServices();
builder.Services.AddAntiforgery();
builder.Services.AddHttpContextAccessor();

builder.Services.AddDistributedSqlServerCache(options =>
{
    options.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.SchemaName = "dbo";
    options.TableName = "TokenCache";
});

builder.Services
    .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(options =>
    {
        builder.Configuration.Bind("AzureAd", options);
        options.SaveTokens = true;
        options.TokenValidationParameters.RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
        
        var apiScope = builder.Configuration["DownstreamApi:Scopes"];
        if (!string.IsNullOrEmpty(apiScope))
        {
            options.Scope.Add(apiScope);
        }
    })
    .EnableTokenAcquisitionToCallDownstreamApi(new[] { builder.Configuration["DownstreamApi:Scopes"] ?? string.Empty })
    .AddDistributedTokenCaches();

builder.Services.Configure<OpenIdConnectOptions>(
    OpenIdConnectDefaults.AuthenticationScheme,
    options => options.ResponseMode = "form_post");

builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromDays(14);
    options.SlidingExpiration = true;
});

builder.Services.AddAuthorization();
builder.Services.AddScoped<AuthTokenHandler>();
builder.Services.AddControllersWithViews().AddMicrosoftIdentityUI();

builder.Services.AddHttpClient("Api", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? throw new InvalidOperationException("ApiBaseUrl missing"));
})
.AddHttpMessageHandler<AuthTokenHandler>();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<OrganizationService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapControllers();
app.MapStaticAssets();
app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

await app.RunAsync();
