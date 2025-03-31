using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace RealEstate.Domain.Entities.dbo
{
    [Table("Usuarios", Schema = "dbo")]

    public class Usuarios
    {
        [Key]
        public string Nada { get; set; }
    }
}
