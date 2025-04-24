

using Swashbuckle.AspNetCore.Annotations;

namespace RealEstate.Application.Dtos.identity
{
    /// <summary>
    ///  Parametros para la Autenticarse/Logearse
    /// </summary>
    public class AuthenticationRequest
    {
        /// <example>
        ///  bennydelamparo@gmail.com
        /// </example>
        [SwaggerParameter(Description = "Ingrese el Correo electronico de su cuenta")]
        public string Email { get; set; }
        /// <example>
        ///  123456789
        /// </example>
        [SwaggerParameter(Description = "Ingrese la contraseña correspondiente")]
        public string Password { get; set; }

    }
}
