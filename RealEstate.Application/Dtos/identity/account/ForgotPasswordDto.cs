

using RealEstate.Application.Dtos.identity.account.@base;
using System.ComponentModel.DataAnnotations;

namespace RealEstate.Application.Dtos.identity.account
{
    public class ForgotPasswordDto : AccountBaseDto
    {
        [Required(ErrorMessage = "Debe colocar su correo.")]
        [DataType(DataType.Text)]
        public string Email { get; set; }
    }
}
