

using RealEstate.Application.Dtos.identity.account.@base;
using System.ComponentModel.DataAnnotations;

namespace RealEstate.Application.Dtos.identity.account
{
    public class LoginDto : AccountBaseDto
    {
        [Required(ErrorMessage = "Debe colocar el correo del usuario.")]
        [DataType(DataType.Text)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Debe colocar una contraseña.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
