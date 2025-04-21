using AutoMapper;
using RealEstate.Application.Dtos;
using RealEstate.Application.Dtos.dbo;
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

        }
    }
}
