

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

            CreateMap<Pagos, PagosDto>()
                .ReverseMap();

            CreateMap<Propiedades, PropiedadesDto>()
                .ForMember(dest => dest.Imagen, opt => opt.Ignore())
                .ReverseMap(); 

            CreateMap<PropiedadFotos, PropiedadFotosDto>()
                .ReverseMap();

            CreateMap<Reservas, ReservasDto>()
                .ReverseMap();

            //CreateMap<PropiedadesDto, PropiedadFotosDto>()
            //    .ForMember(dest => dest.Files, opt => opt.Ignore())
            //    .ForMember(dest => dest.RelacionID, opt => opt.Ignore())
            //    .ReverseMap()
            //    .ForMember(dest => dest.Codigo, opt => opt.Ignore())
            //    .ForMember(dest => dest.AgenteID, opt => opt.Ignore())
            //    .ForMember(dest => dest.Titulo, opt => opt.Ignore())
            //    .ForMember(dest => dest.Descripcion, opt => opt.Ignore())
            //    .ForMember(dest => dest.Precio, opt => opt.Ignore())
            //    .ForMember(dest => dest.Direccion, opt => opt.Ignore())
            //    .ForMember(dest => dest.Ciudad, opt => opt.Ignore())
            //    .ForMember(dest => dest.Sector, opt => opt.Ignore())
            //    .ForMember(dest => dest.CodigoPostal, opt => opt.Ignore())
            //    .ForMember(dest => dest.TotalNivel, opt => opt.Ignore())
            //    .ForMember(dest => dest.Piso, opt => opt.Ignore())
            //    .ForMember(dest => dest.AñoConstruccion, opt => opt.Ignore())
            //    .ForMember(dest => dest.TipoPropiedad, opt => opt.Ignore())
            //    .ForMember(dest => dest.Disponibilidad, opt => opt.Ignore());

        }
    }
}
