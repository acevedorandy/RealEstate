using RealEstate.Application.Dtos.dbo;
using RealEstate.Application.Helpers.web;


namespace RealEstate.Web.Middlewares
{
    public class ValidateUserSesion
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ValidateUserSesion(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public bool HasUser()
        {
            UsuariosDto usuarios = _httpContextAccessor.HttpContext.Session.Get<UsuariosDto>("usuario");

            if (usuarios == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
