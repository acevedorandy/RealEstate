using Microsoft.Extensions.DependencyInjection;
using RealEstate.Application.Contracts.dbo;
using RealEstate.Application.Mapping.dbo;
using RealEstate.Application.Services.dbo;
using RealEstate.Persistance.Interfaces.dbo;
using RealEstate.Persistance.Repositories.dbo;
using RealEstate.Persistance.Validations;

namespace RealEstate.IOC.Dependencies.dbo
{
    public static class DboDependency
    {
        public static void AddDboDependency(this IServiceCollection services)
        {
            // Repositorios 
            services.AddTransient<IContratosRepository, ContratosRepository>();
            services.AddTransient<IFavoritosRepository, FavoritosRepository>();
            services.AddTransient<IMensajesRepository, MensajesRepository>();
            services.AddTransient<IPagosRepository, PagosRepository>();
            services.AddTransient<IPropiedadesRepository, PropiedadesRepository>();
            services.AddTransient<IPropiedadFotosRepository, PropiedadFotosRepository>();
            services.AddTransient<IReservasRepository, ReservasRepository>();
            //services.AddTransient<IUsuariosRepository, UsuariosRepository>();

            // Servicios
            services.AddScoped<IContratosService, ContratosService>();
            services.AddScoped<IFavoritosService, FavoritosService>();
            services.AddScoped<IMensajesService, MensajesService>();
            services.AddScoped<IPagosService, PagosService>();
            services.AddScoped<IPropiedadesService, PropiedadesService>();
            services.AddScoped<IReservasService, ReservasService>();
            services.AddScoped<IUsuariosService, UsuariosService>();

            // Validaciones
            services.AddScoped<MensajesValidate>();
            services.AddScoped<PagosValidate>();
            services.AddScoped<PropiedadesValidate>();
            services.AddScoped<ReservasValidate>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddAutoMapper(typeof(DboMapping));

        }
    }
}
