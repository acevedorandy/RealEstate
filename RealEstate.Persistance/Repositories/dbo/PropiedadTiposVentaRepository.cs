

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
    public sealed class PropiedadTiposVentaRepository(RealEstateContext realEstateContext,
                                                      ILogger<PropiedadTiposVentaRepository> logger) : BaseRepository<PropiedadTiposVenta>(realEstateContext), IPropiedadTiposVentaRepository
    {
        private readonly RealEstateContext _realEstateContext = realEstateContext;
        private readonly ILogger<PropiedadTiposVentaRepository> _logger = logger;

        public async override Task<OperationResult> Save(PropiedadTiposVenta propiedadTiposVenta)
        {
            OperationResult result = new OperationResult();

            try
            {
                result = await base.Save(propiedadTiposVenta);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error guardando la relacion.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> Update(PropiedadTiposVenta propiedadTiposVenta)
        {
            OperationResult result = new OperationResult();

            try
            {
                PropiedadTiposVenta? PropiedadTiposVentaToUpdate = await _realEstateContext.PropiedadTiposVenta.FindAsync(propiedadTiposVenta.PropiedadTipoVentaID);

                PropiedadTiposVentaToUpdate.PropiedadTipoVentaID = propiedadTiposVenta.PropiedadTipoVentaID;
                PropiedadTiposVentaToUpdate.PropiedadID = propiedadTiposVenta.PropiedadID;
                PropiedadTiposVentaToUpdate.TipoVentaID = propiedadTiposVenta.TipoVentaID;

                result = await base.Update(PropiedadTiposVentaToUpdate);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error actualizando la relacion.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> Remove(PropiedadTiposVenta propiedadTiposVenta)
        {
            OperationResult result = new OperationResult();

            try
            {
                result = await base.Remove(propiedadTiposVenta);
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
                                         PropiedadMejoraID = propiedadMejora.MejoraID,
                                         PropiedadID = propiedadMejora.PropiedadID,
                                         MejoraID = propiedadMejora.MejoraID,

                                     }).AsNoTracking()
                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo las relaciones.";
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
                                         PropiedadMejoraID = propiedadMejora.MejoraID,
                                         PropiedadID = propiedadMejora.PropiedadID,
                                         MejoraID = propiedadMejora.MejoraID,

                                     }).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo la relacion.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }
    }
}
