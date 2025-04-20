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
    public class MejorasRepository(RealEstateContext realEstateContext, 
                                   ILogger<MejorasRepository> logger) : BaseRepository<Mejoras>(realEstateContext), IMejorasRepository
    {
        private readonly RealEstateContext _realEstateContext = realEstateContext;
        private readonly ILogger<MejorasRepository> _logger = logger;

        public async override Task<OperationResult> Save(Mejoras mejoras)
        {
            OperationResult result = new OperationResult();

            try
            {
                result = await base.Save(mejoras);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error guardando la mejora.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> Update(Mejoras mejoras)
        {
            OperationResult result = new OperationResult();

            try
            {
                Mejoras? mejorasToUpdate = await _realEstateContext.Mejoras.FindAsync(mejoras.MejoraID);

                mejorasToUpdate.MejoraID = mejoras.MejoraID;
                mejorasToUpdate.Nombre = mejoras.Nombre;

                result = await base.Update(mejorasToUpdate);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error actualizando la mejora.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> Remove(Mejoras mejoras)
        {
            OperationResult result = new OperationResult();

            try
            {
                result = await base.Remove(mejoras);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error eliminando la mejora.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }
        public async override Task<OperationResult> GetAll()
        {
            OperationResult result = new OperationResult();

            try
            {
                result.Data = await (from mejora in _realEstateContext.Mejoras
                                     
                                     select new MejorasModel 
                                     {
                                         MejoraID = mejora.MejoraID,
                                         Nombre = mejora.Nombre,

                                     }).AsNoTracking()
                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo las mejoras.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> GetById(int id)
        {
            OperationResult result = new OperationResult();

            try
            {
                result.Data = await (from mejora in _realEstateContext.Mejoras

                                     where mejora.MejoraID == id

                                     select new MejorasModel
                                     {
                                         MejoraID = mejora.MejoraID,
                                         Nombre = mejora.Nombre,

                                     }).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo las mejoras.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }
    }
}
