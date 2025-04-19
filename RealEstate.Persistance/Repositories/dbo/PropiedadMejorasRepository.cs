using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RealEstate.Domain.Entities.dbo;
using RealEstate.Domain.Result;
using RealEstate.Persistance.Base;
using RealEstate.Persistance.Context;
using RealEstate.Persistance.Interfaces.dbo;
using RealEstate.Persistance.Models.dbo;

namespace RealEstate.Persistance.Repositories.dbo
{
    public class PropiedadMejorasRepository(RealEstateContext realEstateContext,
                                            ILogger<PropiedadMejorasRepository> logger) : BaseRepository<PropiedadMejoras>(realEstateContext), IPropiedadMejorasRepository
    {
        private readonly RealEstateContext _realEstateContext = realEstateContext;
        private readonly ILogger<PropiedadMejorasRepository> _logger = logger;

        public async override Task<OperationResult> Save(PropiedadMejoras propiedadMejoras)
        {
            OperationResult result = new OperationResult();

            try
            {
                result = await base.Save(propiedadMejoras);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error guardando la relacion.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> Update(PropiedadMejoras propiedadMejoras)
        {
            OperationResult result = new OperationResult();

            try
            {
                PropiedadMejoras? propiedadMejorasToUpdate = await _realEstateContext.PropiedadMejoras.FindAsync(propiedadMejoras.PropiedadMejoraID);

                propiedadMejorasToUpdate.PropiedadMejoraID = propiedadMejoras.PropiedadMejoraID;
                propiedadMejorasToUpdate.PropiedadID = propiedadMejoras.PropiedadMejoraID;
                propiedadMejorasToUpdate.MejoraID = propiedadMejoras.MejoraID;

                result = await base.Update(propiedadMejorasToUpdate);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error actualizando la relacion.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> Remove(PropiedadMejoras propiedadMejoras)
        {
            OperationResult result = new OperationResult();

            try
            {
                result = await base.Remove(propiedadMejoras);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error eliminando la relacion.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }
        public async override Task<OperationResult> GetAll()
        {
            OperationResult result = new OperationResult();

            try
            {
                result.Data = await (from propiedadMejora in _realEstateContext.PropiedadMejoras
                                     
                                     select new PropiedadMejorasModel
                                     {
                                         PropiedadMejoraID = propiedadMejora.PropiedadMejoraID,
                                         PropiedadID = propiedadMejora.PropiedadID,
                                         MejoraID = propiedadMejora.MejoraID

                                     }).AsNoTracking()
                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo las relaciones entre la propiedad y sus mejoras";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> GetById(int id)
        {
            OperationResult result = new OperationResult();

            try
            {
                result.Data = await (from propiedadMejora in _realEstateContext.PropiedadMejoras

                                     where propiedadMejora.PropiedadMejoraID == id

                                     select new PropiedadMejorasModel
                                     {
                                         PropiedadMejoraID = propiedadMejora.PropiedadMejoraID,
                                         PropiedadID = propiedadMejora.PropiedadID,
                                         MejoraID = propiedadMejora.MejoraID

                                     }).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo la relacion entre la propiedad y su mejora.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }
    }
}
