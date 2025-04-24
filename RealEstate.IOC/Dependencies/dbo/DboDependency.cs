using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RealEstate.Application.Contracts.dbo;
using RealEstate.Application.Features.agente.Commands;
using RealEstate.Application.Features.agente.Queries.GetAllAgente;
using RealEstate.Application.Features.agente.Queries.GetAllPropertyByAgent;
using RealEstate.Application.Features.agente.Queries.GetByIDAgente;
using RealEstate.Application.Features.mejora.Commands.RemoveMejoras;
using RealEstate.Application.Features.mejora.Commands.SaveMejoras;
using RealEstate.Application.Features.mejora.Commands.UpdateMejoras;
using RealEstate.Application.Features.mejora.Queries.GetAllMejoras;
using RealEstate.Application.Features.mejora.Queries.GetByIDMejoras;
using RealEstate.Application.Features.propiedad.Queries.GetAllPropiedades;
using RealEstate.Application.Features.propiedad.Queries.GetByCodePropiedades;
using RealEstate.Application.Features.propiedad.Queries.GetByIDPropiedades;
using RealEstate.Application.Features.tipoPropiedad.Commands.RemoveTiposPropiedad;
using RealEstate.Application.Features.tipoPropiedad.Commands.SaveTiposPropiedad;
using RealEstate.Application.Features.tipoPropiedad.Commands.UpdateTiposPropiedad;
using RealEstate.Application.Features.tipoPropiedad.Queries.GetAllTiposPropiedad;
using RealEstate.Application.Features.tipoPropiedad.Queries.GetByIDTiposPropiedad;
using RealEstate.Application.Features.tipoVenta.Commands.RemoveTiposVenta;
using RealEstate.Application.Features.tipoVenta.Commands.SaveTiposVenta;
using RealEstate.Application.Features.tipoVenta.Commands.UpdateTiposVenta;
using RealEstate.Application.Features.tipoVenta.Queries.GetAllPropiedades;
using RealEstate.Application.Features.tipoVenta.Queries.GetByIDPropiedades;
using RealEstate.Application.Mapping.dbo;
using RealEstate.Application.Services.dbo;
using RealEstate.Persistance.Interfaces.dbo;
using RealEstate.Persistance.Repositories.dbo;
using RealEstate.Persistance.Validations;

namespace RealEstate.IOC.Dependencies.dbo
{
    public static class DboDependency
    {
        private static void AddDboDependency(this IServiceCollection services)
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
            services.AddTransient<ITiposPropiedadRepository, TiposPropiedadRepository>();
            services.AddTransient<ITiposVentaRepository, TiposVentaRepository>();
            services.AddTransient<IReservasRepository, ReservasRepository>();

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
            services.AddScoped<ITiposPropiedadService, TiposPropiedadService>();
            services.AddScoped<ITiposVentaService, TiposVentaService>();
            services.AddScoped<IReservasService, ReservasService>();

            // Validaciones
            services.AddScoped<MensajesValidate>();
            services.AddScoped<PagosValidate>();
            services.AddScoped<PropiedadesValidate>();
            services.AddScoped<ReservasValidate>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddAutoMapper(typeof(DboMapping));

            

            //services.AddMediatR(typeof(DboMapping));
        }

        private static void AddMediatRDependencies(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            #region Agente
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllAgenteQuery).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllAgenteQueryHandler).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllPropertyByAgentQuery).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllPropertyByAgentQueryHandler).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetByIDAgenteQuery).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetByIDAgenteQueryHandler).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ChangeStatusAgenteCommand).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ChangeStatusAgenteCommandHandler).Assembly));
            #endregion

            #region Mejoras
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllMejorasQuery).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllMejorasQueryHandler).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetByIDMejorasQuery).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetByIDMejorasQueryHandler).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(SaveMejorasCommand).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(SaveMejorasCommandHandler).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UpdateMejorasCommand).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UpdateMejorasCommandHandler).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RemoveMejorasCommand).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RemoveMejorasCommandHandler).Assembly));
            #endregion

            #region Propiedades
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllPropiedadesQuery).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllPropiedadesQueryHandler).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetByCodePropiedadesQuery).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetByIDPropiedadesQuery).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetByIDPropiedadesHandler).Assembly));
            #endregion

            #region TipoPropiedades
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllTiposPropiedadQuery).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllTiposPropiedadCommandHandler).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetByIDTiposPropiedadQuery).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetByIDTiposPropiedadCommandHandler).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(SaveTiposPropiedadCommand).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(SaveTiposPropiedadCommandHandler).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UpdateTiposPropiedadCommand).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UpdateTiposPropiedadCommandHandler).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RemoveTiposPropiedadCommand).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UpdateTiposPropiedadCommandHandler).Assembly));
            #endregion

            #region TipoVentas
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllTiposVentaQuery).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllTiposVentaQueryHandler).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetByIDTiposVentaQuery).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetByIDTiposVentaQueryHandler).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(SaveTiposVentaCommand).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(SaveTiposVentaCommandHandler).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UpdateTiposVentaCommand).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UpdateTiposVentaCommandHandler).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RemoveTiposVentaCommand).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RemoveTiposVentaCommandHandler).Assembly));
            #endregion
        }

        public static void AddDboDependencyForApi(this IServiceCollection services)
        {
            AddDboDependency(services);
            services.AddTransient<IUsuariosRepository, UsuariosRepository>();
            AddMediatRDependencies(services);
        }
        public static void AddDboDependencyForWebApp(this IServiceCollection services)
        {
            AddDboDependency(services);
            services.AddTransient<IUsuariosRepository, UsuariosRepository>();
            services.AddScoped<IUsuariosService, UsuariosService>();
        }
    }
}
