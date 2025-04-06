using Microsoft.EntityFrameworkCore;
using RealEstate.Identity.Dependency;
using RealEstate.IOC.Dependencies.dbo;
using RealEstate.IOC.Dependencies.infraestructure;
using RealEstate.Persistance.Context;
using RealEstate.Web.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<RealEstateContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("RealEstate")));

// Registro de las dependencias de Identity
builder.Services.AddIdentityDependency(builder.Configuration);

// Registro de las dependencias del servicio de Identity
builder.Services.AddIdentityService();

// Registro de las dependencias del esquema Dbo
builder.Services.AddDboDependency();

// Registro de las dependencias de la capa Infraestructure
builder.Services.AddEmailDependency(builder.Configuration);

// Registro de las dependencias de la web
builder.Services.AddScoped<LoginAuthorize>();
builder.Services.AddScoped<ValidateUserSesion>();

// Registro para las sesiones
builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    await scope.ServiceProvider.RunIdentitySeeds();
}

app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Index}/{id?}");

await app.RunAsync();
