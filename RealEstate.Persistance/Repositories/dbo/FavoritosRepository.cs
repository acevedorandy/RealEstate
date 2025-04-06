

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
                favoritosToUpdate.IsFavoritos = favoritos.IsFavoritos;

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
                                 IsFavoritos = favorito.IsFavoritos,

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
                                 IsFavoritos = favorito.IsFavoritos,

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
    }
}
