using IdentityProvider;
using IdentityProvider.Data;
using IdentityProvider.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<IdentityProviderDbContext>(o =>
    o.UseSqlite(builder.Configuration.GetConnectionString(nameof(IdentityProvider)),
        b => b.MigrationsAssembly(nameof(IdentityProvider))));

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddIdentityServer()
    .AddProfileService<ProfileService>()
    .AddInMemoryIdentityResources(Config.IdentityResources)
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddInMemoryClients(Config.Clients);

builder.Services.AddRazorPages();

var app = builder.Build();

app.UseStaticFiles();
app.UseIdentityServer();

app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    DbInitializer.ConfigureDb(scope, Config.Users);
}

app.Run();
