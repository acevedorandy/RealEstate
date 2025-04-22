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
            services.AddTransient<IMejorasRepository, MejorasRepository>();
            services.AddTransient<IMensajesRepository, MensajesRepository>();
            services.AddTransient<IOfertasRepository, OfertasRepository>();
            services.AddTransient<IPagosRepository, PagosRepository>();
            services.AddTransient<IPropiedadesRepository, PropiedadesRepository>();
            services.AddTransient<IPropiedadMejorasRepository, PropiedadMejorasRepository>();
            services.AddTransient<IPropiedadFotosRepository, PropiedadFotosRepository>();
            services.AddTransient<IPropiedadTiposVentaRepository, PropiedadTiposVentaRepository>();
            services.AddTransient<IReservasRepository, ReservasRepository>();
            services.AddTransient<ITiposPropiedadRepository, TiposPropiedadRepository>();
            services.AddTransient<ITiposVentaRepository, TiposVentaRepository>();
            services.AddTransient<IUsuariosRepository, UsuariosRepository>();

            // Servicios
            services.AddScoped<IContratosService, ContratosService>();
            services.AddScoped<IFavoritosService, FavoritosService>();
            services.AddScoped<IMejorasService, MejorasService>();
            services.AddScoped<IMensajesService, MensajesService>();
            services.AddScoped<IOfertasService, OfertasService>();
            services.AddScoped<IPagosService, PagosService>();
            services.AddScoped<IPropiedadFotosService, PropiedadFotosService>();
            services.AddScoped<IPropiedadMejorasService, PropiedadMejorasService>();
            services.AddScoped<IPropiedadTiposVentaService, PropiedadTiposVentaService>();
            services.AddScoped<IPropiedadesService, PropiedadesService>();
            services.AddScoped<IReservasService, ReservasService>();
            services.AddScoped<ITiposPropiedadService, TiposPropiedadService>();
            services.AddScoped<ITiposVentaService, TiposVentaService>();
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
