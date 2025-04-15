

using AutoMapper;
using RealEstate.Application.Dtos.dbo;
using RealEstate.Domain.Entities.dbo;

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

            CreateMap<Mensajes, MensajesDto>()
                .ReverseMap();

            CreateMap<Ofertas, OfertasDto>()
                .ReverseMap();

            CreateMap<Pagos, PagosDto>()
                .ReverseMap();

            CreateMap<Propiedades, PropiedadesDto>()
                .ForMember(dest => dest.Imagen, opt => opt.Ignore())
                .ReverseMap(); 

            CreateMap<PropiedadFotos, PropiedadFotosDto>()
                .ReverseMap();

            CreateMap<Reservas, ReservasDto>()
                .ReverseMap();

        }
    }
}
