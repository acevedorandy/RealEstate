using AutoMapper;
using RealEstate.Application.Dtos;
using RealEstate.Application.Dtos.dbo;
using RealEstate.Application.Features.mejora.Commands.SaveMejoras;
using RealEstate.Application.Features.mejora.Commands.UpdateMejoras;
using RealEstate.Application.Features.tipoPropiedad.Commands.SaveTiposPropiedad;
using RealEstate.Application.Features.tipoPropiedad.Commands.UpdateTiposPropiedad;
using RealEstate.Application.Features.tipoVenta.Commands.SaveTiposVenta;
using RealEstate.Application.Features.tipoVenta.Commands.UpdateTiposVenta;
using RealEstate.Application.Models;
using RealEstate.Domain.Entities.dbo;
using RealEstate.Persistance.Models.dbo;

namespace RealEstate.Application.Mapping.dbo
{
    public class DboMapping : Profile
    {
        public DboMapping()
        {
            CreateMap<Contratos, ContratosDto>()
                .ReverseMap();

            CreateMap<Favoritos, FavoritosDto>()
                .ReverseMap();

            CreateMap<Mejoras, MejorasDto>()
                .ReverseMap();

            CreateMap<Mensajes, MensajesDto>()
                .ReverseMap();

            CreateMap<Ofertas, OfertasDto>()
                .ReverseMap();

            CreateMap<Ofertas, OfertasModel>()
                .ReverseMap();

            CreateMap<Pagos, PagosDto>()
                .ReverseMap();

            CreateMap<Propiedades, PropiedadesDto>()
                .ForMember(dest => dest.Imagen, opt => opt.Ignore())
                .ReverseMap(); 

            CreateMap<PropiedadFotos, PropiedadFotosDto>()
                .ReverseMap();

            CreateMap<PropiedadMejoras, PropiedadMejorasDto>()
                .ReverseMap();

            CreateMap<PropiedadTiposVenta, PropiedadTiposVentaDto>()
                .ReverseMap();

            CreateMap<Reservas, ReservasDto>()
                .ReverseMap();

            CreateMap<TiposPropiedad, TiposPropiedadDto>()
                .ReverseMap();

            CreateMap<TiposVenta, TiposVentaDto>()
                .ReverseMap();
            CreateMap<TiposVentaModel, TiposVenta>()
                .ReverseMap();

            CreateMap<TiposPropiedadModel, TiposPropiedad>()
                .ReverseMap();
            CreateMap<TiposPropiedad, TiposPropiedadModel>()
                .ReverseMap();

            /* Modelado Mapping*/
            CreateMap<PropiedadesModel, PropiedadesViewModel>()
                .ReverseMap();

            CreateMap<SaveTiposPropiedadCommand, TiposPropiedad>()
                .ReverseMap();
            CreateMap<UpdateTiposPropiedadCommand, TiposPropiedad>()
                .ReverseMap();
            CreateMap<UpdateTiposPropiedadCommand, TiposPropiedadModel>()
                .ReverseMap();
            CreateMap<TiposPropiedadModel, UpdateMejorasCommand>()
                .ReverseMap();

            CreateMap<SaveTiposVentaCommand, TiposVenta>()
                .ForMember(x => x.TipoVentaID, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<UpdateTiposVentaCommand, TiposVenta>()
                .ReverseMap();
            CreateMap<UpdateTiposVentaCommand, TiposVentaModel>()
                .ReverseMap();
            CreateMap<TiposVentaModel, UpdateTiposVentaCommand>()
                .ReverseMap();

            CreateMap<SaveMejorasCommand, Mejoras>()
                .ForMember(x => x.MejoraID, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<UpdateMejorasCommand, Mejoras>()
                .ReverseMap();
            CreateMap<UpdateMejorasCommand, MejorasModel>()
                .ReverseMap();
            CreateMap<MejorasModel, UpdateMejorasCommand>()
                .ReverseMap();
            CreateMap<Mejoras, MejorasModel>()
                .ReverseMap();
            CreateMap<MejorasModel, Mejoras>()
                .ReverseMap();

        }
    }
}
