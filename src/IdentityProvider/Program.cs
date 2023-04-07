using IdentityProvider;
using IdentityProvider.Data;
using IdentityProvider.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<IdentityProviderDbContext>(o =>
    o.UseSqlite(builder.Configuration.GetConnectionString(nameof(IdentityProvider)),
        b => b.MigrationsAssembly(nameof(IdentityProvider))));

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<IdentityProviderDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddIdentityServer()
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddInMemoryClients(Config.Clients)
    .AddAspNetIdentity<User>();

builder.Services.AddRazorPages();

var app = builder.Build();

app.UseStaticFiles();
app.UseIdentityServer();

app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    await DbStartup.ConfigureDb(scope, Config.Users);
}

app.Run();
