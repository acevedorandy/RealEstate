using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using RealEstate.Api.Extensions;
using RealEstate.Identity.Dependency;
using RealEstate.IOC.Dependencies.dbo;
using RealEstate.IOC.Dependencies.infraestructure;
using RealEstate.Persistance.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<RealEstateContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("RealEstateDB")));

//Dependecies dbo
builder.Services.AddDboDependency();

//Dependecies Identity
builder.Services.AddIdentityDependency(builder.Configuration);
builder.Services.AddIdentityService();

//Dependencies Infraestructure
builder.Services.AddEmailDependency(builder.Configuration);
builder.Services.AddJWTokenDependency(builder.Configuration);

//Others Dependencies for API
builder.Services.AddHealthChecks();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddApiVersioningExtesion();
builder.Services.AddSwaggerExtension();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Obtener el provider
var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler();
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    await scope.ServiceProvider.RunIdentitySeeds();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// ⚡ Solo esta llamada para Swagger UI:
app.UseSwaggerExtension(apiVersionDescriptionProvider);

app.UseHealthChecks("/health");
app.UseSession();

app.MapControllers();
app.Run();
