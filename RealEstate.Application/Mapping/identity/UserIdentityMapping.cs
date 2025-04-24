

using AutoMapper;
using RealEstate.Application.Dtos.identity.account;
using RealEstate.Application.Dtos.identity;
using RealEstate.Application.Dtos.dbo;
using RealEstate.Identity.Shared.Entities;

namespace RealEstate.Application.Mapping.identity
{
    public class UserIdentityMapping : Profile
    {
        public UserIdentityMapping()
        {
            CreateMap<AuthenticationRequest, LoginDto>()
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<RegisterRequest, RegisterDto>()
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<ForgotPasswordRequest, ForgotPasswordDto>()
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<ResetPasswordRequest, ResetPasswordDto>()
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<RegisterRequest, RegisterDto>()
                .ReverseMap();

            CreateMap<ApplicationUser, UsuariosDto>()
                .ReverseMap();

            CreateMap<ApplicationUser, RegisterDto>()
                .ReverseMap();
        }
    }
}
