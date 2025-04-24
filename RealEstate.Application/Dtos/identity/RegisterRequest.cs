

using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace RealEstate.Application.Dtos.identity
{
    /// <summary>
    ///  Parametros para la Registrar un usuario
    /// </summary>
    public class RegisterRequest
    {
        /// <example>
        ///  Randy
        /// </example>

        [SwaggerParameter(Description = "Ingrese su nombre")]
        public string Nombre { get; set; }
        /// <example>
        ///  Acevedo
        /// </example>
        [SwaggerParameter(Description = "Ingrese su apellido")]
        public string Apellido { get; set; }
        public string? Foto { get; set; }
        /// <example>
        ///  456-9874561-6
        /// </example>
        [SwaggerParameter(Description = "Ingrese su cedula")]
        public string Cedula { get; set; }
        /// <example>
        ///  randyacevedo@gmail.com
        /// </example>
        [SwaggerParameter(Description = "Ingrese el Correo electronico de su cuenta")]
        public string Email { get; set; }
        /// <example>
        ///  RYAC
        /// </example>
        [SwaggerParameter(Description = "Ingrese un nombre de usuario")]
        public string UserName { get; set; }
        /// <example>
        ///  cyvuybn
        /// </example>
        [SwaggerParameter(Description = "Ingrese su contraseña")]
        public string Password { get; set; }
        /// <example>
        ///  cyvuybn
        /// </example>
        [SwaggerParameter(Description = "Confimme la contraseña")]
        public string ConfirmPassword { get; set; }
        /// <example>
        ///  849-531-4562
        /// </example>
        [SwaggerParameter(Description = "Ingrese un telefono")]
        public string Phone { get; set; }
        /// <example>
        ///  Desarrollador
        /// </example>
        [SwaggerParameter(Description = "Rol del usuario (Suele ser automatico)")]
        [JsonIgnore]
        public string? Rol { get; set; }
        [JsonIgnore]
        public bool IsActive { get; set; } 
        public IFormFile FotoFile { get; set; }

    }
}
