

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RealEstate.Domain.Entities.dbo;
using RealEstate.Domain.Result;
using RealEstate.Identity.Shared.Context;
using RealEstate.Persistance.Base;
using RealEstate.Persistance.Context;
using RealEstate.Persistance.Interfaces.dbo;
using RealEstate.Persistance.Models.dbo;

namespace RealEstate.Persistance.Repositories.dbo
{
    public sealed class FavoritosRepository(RealEstateContext realEstateContext, IdentityContext identityContext,
                                            ILogger<FavoritosRepository> logger) : BaseRepository<Favoritos>(realEstateContext), IFavoritosRepository
    {
        private readonly RealEstateContext _realEstateContext = realEstateContext;
        private readonly ILogger<FavoritosRepository> _logger = logger;
        private readonly IdentityContext _identityContext = identityContext;

        public async override Task<OperationResult> Save(Favoritos favoritos)
        {
            OperationResult result = new OperationResult();

            try
            {
                result = await base.Save(favoritos);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error guardando el favorito.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> Update(Favoritos favoritos)
        {
            OperationResult result = new OperationResult();

            try
            {
                Favoritos? favoritosToUpdate = await _realEstateContext.Favoritos.FindAsync(favoritos.FavoritoID);

                favoritosToUpdate.FavoritoID = favoritos.FavoritoID;
                favoritosToUpdate.UsuarioID = favoritos.UsuarioID;
                favoritosToUpdate.PropiedadID = favoritos.PropiedadID;
                favoritosToUpdate.IsFavorito = favoritos.IsFavorito;

                result = await base.Update(favoritosToUpdate);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error actualizando el favorito.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> Remove(Favoritos favoritos)
        {
            OperationResult result = new OperationResult();

            try
            {
                result = await base.Remove(favoritos);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error eliminando el favorito";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> GetAll()
        {
            OperationResult result = new OperationResult();

            try
            {
                var usuarios = await _identityContext.Users
                    .ToListAsync();

                var propiedades = await _realEstateContext.Propiedades
                    .ToListAsync();

                var favoritos = await _realEstateContext.Favoritos
                    .ToListAsync();

                var datos = (from favorito in favoritos
                             join usuario in usuarios on favorito.UsuarioID equals usuario.Id
                             join propiedad in propiedades on favorito.PropiedadID equals propiedad.PropiedadID

                             select new FavoritosModel
                             {
                                 FavoritoID = favorito.FavoritoID,
                                 UsuarioID = usuario.Id,
                                 PropiedadID = propiedad.PropiedadID,
                                 IsFavorito = favorito.IsFavorito,

                             }).ToList();

                result.Data = datos;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo los favoritos";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> GetById(int id)
        {
            OperationResult result = new OperationResult();

            try
            {
                var usuarios = await _identityContext.Users
                    .ToListAsync();

                var propiedades = await _realEstateContext.Propiedades
                    .ToListAsync();

                var favoritos = await _realEstateContext.Favoritos
                    .ToListAsync();

                var datos = (from favorito in favoritos
                             join usuario in usuarios on favorito.UsuarioID equals usuario.Id
                             join propiedad in propiedades on favorito.PropiedadID equals propiedad.PropiedadID

                             where favorito.FavoritoID == id

                             select new FavoritosModel
                             {
                                 FavoritoID = favorito.FavoritoID,
                                 UsuarioID = usuario.Id,
                                 PropiedadID = propiedad.PropiedadID,
                                 IsFavorito = favorito.IsFavorito,

                             }).FirstOrDefault();

                result.Data = datos;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo el favorito";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        //public async Task<OperationResult> GetPropertyByID(int propiedadId)
        //{
        //    OperationResult result = new OperationResult();

        //    try
        //    {
        //        var propiedades = await _realEstateContext.Propiedades
        //            .ToListAsync();

        //        var usuarios = await _identityContext.Users
        //            .ToListAsync();

        //        var datos = (from propiedad in propiedades
        //                     join agente in usuarios on propiedad.AgenteID equals agente.Id

        //                     where propiedad.PropiedadID == propiedadId

        //                     select new PropiedadesModel()
        //                     {
        //                         PropiedadID = propiedad.PropiedadID,
        //                         Codigo = propiedad.Codigo,
        //                         AgenteID = agente.Id,
        //                         Titulo = propiedad.Titulo,
        //                         Descripcion = propiedad.Descripcion,
        //                         Precio = propiedad.Precio,
        //                         Direccion = propiedad.Direccion,
        //                         Ciudad = propiedad.Ciudad,
        //                         Sector = propiedad.Sector,
        //                         CodigoPostal = propiedad.CodigoPostal,
        //                         Habitaciones = propiedad.Habitaciones,
        //                         Baños = propiedad.Baños,
        //                         Parqueos = propiedad.Parqueos,
        //                         TamañoTerreno = propiedad.TamañoTerreno,
        //                         TotalNivel = propiedad.TotalNivel,
        //                         Piso = propiedad.Piso,
        //                         AñoConstruccion = propiedad.AñoConstruccion,
        //                         TipoPropiedad = propiedad.TipoPropiedad,
        //                         Disponibilidad = propiedad.Disponibilidad,
        //                         Imagen = propiedad.Imagen

        //                     }).FirstOrDefault();

        //        result.Data = datos;
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Success = false;
        //        result.Message = "Ha ocurrido un error obteniendo la propiedad";
        //        logger.LogError(result.Message, ex.ToString());
        //    }
        //    return result;
        //}

        public async Task<bool> ExistsRelation(int propiedadId, string userId)
        {
            var relation = await _realEstateContext.Favoritos
                .Where(f => f.PropiedadID == propiedadId && f.UsuarioID == userId)
                .FirstOrDefaultAsync();

            return relation != null;
        }

        public async Task<OperationResult> GetAllFavoritePropertyByUser(string userId)
        {
            OperationResult result = new OperationResult();

            try
            {
                var usuarios = await _identityContext.Users
                    .ToListAsync();

                var propiedades = await _realEstateContext.Propiedades
                    .ToListAsync();

                var favoritos = await _realEstateContext.Favoritos
                    .ToListAsync();

                var datos = (from favorito in favoritos
                             join usuario in usuarios on favorito.UsuarioID equals usuario.Id
                             join propiedad in propiedades on favorito.PropiedadID equals propiedad.PropiedadID

                             where favorito.UsuarioID == userId && propiedad.Vendida == false

                             select new PropiedadesModel
                             {
                                 PropiedadID = propiedad.PropiedadID,
                                 Codigo = propiedad.Codigo,
                                 AgenteID = usuario.Id,
                                 Titulo = propiedad.Titulo,
                                 Descripcion = propiedad.Descripcion,
                                 Precio = propiedad.Precio,
                                 Direccion = propiedad.Direccion,
                                 Ciudad = propiedad.Ciudad,
                                 Sector = propiedad.Sector,
                                 CodigoPostal = propiedad.CodigoPostal,
                                 Habitaciones = propiedad.Habitaciones,
                                 Baños = propiedad.Baños,
                                 Parqueos = propiedad.Parqueos,
                                 TamañoTerreno = propiedad.TamañoTerreno,
                                 TotalNivel = propiedad.TotalNivel,
                                 Piso = propiedad.Piso,
                                 AñoConstruccion = propiedad.AñoConstruccion,
                                 TipoPropiedad = propiedad.TipoPropiedad,
                                 Disponibilidad = propiedad.Disponibilidad,
                                 Imagen = propiedad.Imagen

                             }).ToList();

                result.Data = datos;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo los favoritos";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }
    }
}
