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
    public class TiposVentaRepository(RealEstateContext realEstateContext,
                                      ILogger<TiposVentaRepository> logger) : BaseRepository<TiposVenta>(realEstateContext), ITiposVentaRepository
    {
        private readonly RealEstateContext _realEstateContext = realEstateContext;
        private readonly ILogger<TiposVentaRepository> _logger = logger;

        public async override Task<OperationResult> Save(TiposVenta tiposVenta)
        {
            OperationResult result = new OperationResult();

            try
            {
                result = await base.Save(tiposVenta);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error guardando el tipo de venta.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> Update(TiposVenta tiposVenta)
        {
            OperationResult result = new OperationResult();

            try
            {
                TiposVenta? tiposVentaToUpdate = await _realEstateContext.TiposVenta.FindAsync(tiposVenta.TipoVentaID);

                tiposVentaToUpdate.TipoVentaID = tiposVenta.TipoVentaID;
                tiposVentaToUpdate.Nombre = tiposVenta.Nombre;
                tiposVentaToUpdate.Descripcion = tiposVenta.Descripcion;

                result = await base.Update(tiposVentaToUpdate);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error actualizando el tipo de venta.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> Remove(TiposVenta tiposVenta)
        {
            OperationResult result = new OperationResult();

            try
            {
                result = await base.Remove(tiposVenta);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error eliminando el tipo de venta.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> GetAll()
        {
            OperationResult result = new OperationResult();

            try
            {
                result.Data = await (from tipoVenta in _realEstateContext.TiposVenta

                                     select new TiposVentaModel
                                     {
                                         TipoVentaID = tipoVenta.TipoVentaID,
                                         Nombre = tipoVenta.Nombre,
                                         Descripcion = tipoVenta.Descripcion

                                     }).AsNoTracking()
                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo los tipos de ventas.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> GetById(int id)
        {
            OperationResult result = new OperationResult();

            try
            {
                result.Data = await (from tipoVenta in _realEstateContext.TiposVenta

                                     where tipoVenta.TipoVentaID == id

                                     select new TiposVentaModel
                                     {
                                         TipoVentaID = tipoVenta.TipoVentaID,
                                         Nombre = tipoVenta.Nombre,
                                         Descripcion = tipoVenta.Descripcion

                                     }).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo el tipo de venta.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }
    }
}
