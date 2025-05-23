﻿

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstate.Domain.Entities.dbo
{
    [Table("Favoritos", Schema = "dbo")]
    public class Favoritos
    {
        [Key]
        public string UsuarioID { get; set; }
        public int PropiedadID { get; set; }
        public bool IsFavoritos { get; set; }
    }
}
