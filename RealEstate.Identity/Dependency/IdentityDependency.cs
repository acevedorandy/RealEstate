

using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using RealEstate.Application.Contracts.identity;
using RealEstate.Identity.Helpers;
using RealEstate.Identity.Seeds;
using RealEstate.Identity.Services;
using RealEstate.Identity.Shared.Context;
using RealEstate.Identity.Shared.Entities;
using RealEstate.Infraestructure.Settings;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using RealEstate.Application.Responses.identity;

namespace RealEstate.Identity.Dependency
{
    public static class IdentityDependency
    {
        public static void AddIdentityDependency(this IServiceCollection services, IConfiguration configuration)
        {
            Context(services, configuration);

            #region Token Lifetime
            services.Configure<DataProtectionTokenProviderOptions>(opt =>
            {
                opt.TokenLifespan = TimeSpan.FromSeconds(300);
            });
            #endregion

            #region Auth
            services.AddAuthentication(opt =>
            {
                opt.DefaultScheme = IdentityConstants.ApplicationScheme;
                opt.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
                opt.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
            }).AddCookie(IdentityConstants.ApplicationScheme, opt =>
            {
                opt.ExpireTimeSpan = TimeSpan.FromHours(24);
                opt.LoginPath = "/Account";
                opt.AccessDeniedPath = "/Account/AccessDenied";
            });
            #endregion
        }
        public static void AddIdentityDependencyForApi(this IServiceCollection services, IConfiguration configuration)
        {
            Context(services, configuration);

            services.Configure<JWTSettings>(configuration.GetSection("JWTSettings"));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = configuration["JWTSettings:Issuer"],
                    ValidAudience = configuration["JWTSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"]))
                };
                options.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = c =>
                    {
                        c.NoResult();
                        c.Response.StatusCode = 500;
                        c.Response.ContentType = "text/plain";
                        return c.Response.WriteAsync(c.Exception.ToString());
                    },
                    OnChallenge = c =>
                    {
                        c.HandleResponse();
                        c.Response.StatusCode = 401;
                        c.Response.ContentType = "application/json";
                        var result = JsonConvert.SerializeObject(new JwtResponse { HasError = true, Error = "No estas Autenticado" });
                        return c.Response.WriteAsync(result);
                    },
                    OnForbidden = c =>
                    {
                        c.Response.StatusCode = 403;
                        c.Response.ContentType = "application/json";
                        var result = JsonConvert.SerializeObject(new JwtResponse { HasError = true, Error = "No estas Autorizado para esta seccion" });
                        return c.Response.WriteAsync(result);
                    }
                };
            });

            #region Service
            services.AddTransient<IAccountServiceForWebApi, AccountServiceForWebApi>();
            services.AddTransient<JWTHelper>();
            services.AddTransient<EmailHelper>();
            #endregion
        }

        private static void Context(this IServiceCollection services, IConfiguration configuration)
        {
            #region Identity (solo UNA VEZ)
            services.AddIdentityCore<ApplicationUser>(options =>
            {
                // Configuración de seguridad
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 1;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
            .AddRoles<IdentityRole>()
            .AddSignInManager()
            .AddEntityFrameworkStores<IdentityContext>()
            .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>("Default");
            #endregion
            #region Context
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<IdentityContext>(options => options.UseInMemoryDatabase("IdentityDb"));
            }
            else
            {
                services.AddDbContext<IdentityContext>(options =>
                {
                    options.EnableSensitiveDataLogging();
                    options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"),
                        m => m.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName));
                });
            }
            #endregion
        }

        public static async Task RunIdentitySeeds(this IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var service = scope.ServiceProvider;

                try
                {
                    var userManager = service.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();

                    // Seeds
                    await DefaultRoles.SeedAsync(userManager, roleManager);
                    await DefaultAdministradorUser.SeedAsync(userManager, roleManager);
                    await DefaultDesarrolladorUser.SeedAsync(userManager, roleManager);
                    await DefaultAgenteUser.SeedAsync(userManager, roleManager);
                    await DefaultClienteUser.SeedAsync(userManager, roleManager);
                    await SuperAdminUser.SeedAsync(userManager, roleManager);

                    Console.WriteLine("✔️ Seeds ejecutadas correctamente.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error en Seeds: {ex.Message}");
                }
            }
        }

        public static void AddIdentityService(this IServiceCollection services)
        {
            services.AddTransient<IAccountServiceForWebApp, AccountServiceForWebApp>();
            //services.AddTransient<IProfileService, ProfileService>();
            services.AddTransient<EmailHelper>();
        }
    }
}
