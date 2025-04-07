using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealEstate.Infraestructure.Interfaces;
using RealEstate.Infraestructure.Services;
using RealEstate.Infraestructure.Settings;

namespace RealEstate.IOC.Dependencies.infraestructure
{
    public static class EmailDependency
    {
        public static void AddEmailDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            services.AddTransient<IEmailService, EmailService>();
        }
    }
}
