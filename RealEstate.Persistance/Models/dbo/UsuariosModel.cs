

using System.ComponentModel.DataAnnotations;

namespace RealEstate.Persistance.Models.dbo
{
    public class UsuariosModel
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Cedula { get; set; }
        public string Foto { get; set; }
        public string Rol { get; set; }
        public bool IsActive { get; set; }
    }
}
