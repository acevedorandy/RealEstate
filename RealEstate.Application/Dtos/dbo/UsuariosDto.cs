﻿

namespace RealEstate.Application.Dtos.dbo
{
    public class UsuariosDto
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Cedula { get; set; }
        public string Foto { get; set; }
        public bool IsActive { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }

    }
}
