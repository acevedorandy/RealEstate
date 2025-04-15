

using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Entities.dbo;

namespace RealEstate.Persistance.Context
{
    public partial class RealEstateContext : DbContext
    {
        public RealEstateContext(DbContextOptions<RealEstateContext> options) : base(options)
        {
            
        }

        #region
        
        public DbSet<Contratos> Contratos { get; set; }
        public DbSet<Favoritos> Favoritos { get; set; }
        public DbSet<Mensajes> Mensajes { get; set; }
        public DbSet<Ofertas> Ofertas { get; set; }
        public DbSet<Pagos> Pagos { get; set; }
        public DbSet<Propiedades> Propiedades { get; set; }
        public DbSet<PropiedadFotos> PropiedadFotos { get; set; }
        public DbSet<Reservas> Reservas { get; set; }
        public DbSet<Usuarios> Usuarios { get; set; }

        #endregion
    }
}
