using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RealEstate.Domain.Entities.dbo;
using RealEstate.Domain.Result;
using RealEstate.Persistance.Base;
using RealEstate.Persistance.Context;
using RealEstate.Persistance.Interfaces.dbo;
using RealEstate.Persistance.Models.dbo;
using RealEstate.Persistance.Validations;

namespace RealEstate.Persistance.Repositories.dbo
{
    public sealed class PagosRepository(RealEstateContext realEstateContext, ILogger<PagosRepository> logger,
        PagosValidate pagosValidate) : BaseRepository<Pagos>(realEstateContext), IPagosRepository
    {
        private readonly RealEstateContext _realEstateContext = realEstateContext;
        private readonly ILogger<PagosRepository> logger = logger;
        private readonly PagosValidate _pagosValidate = pagosValidate;

        public async override Task<OperationResult> Save(Pagos pagos)
        {
            OperationResult result = new OperationResult();

            try
            {
                _pagosValidate.PagosValidations(result, pagos);

                result = await base.Save(pagos);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error al guardar el pago";
                logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> Update(Pagos pagos)
        {
            OperationResult result = new OperationResult();

            try
            {
                Pagos? pagosToUpdate = await _realEstateContext.Pagos.FindAsync(pagos.PagoID);

                pagosToUpdate.PagoID = pagos.PagoID;
                pagosToUpdate.ContratoID = pagos.ContratoID;
                pagosToUpdate.FechaPago = pagos.FechaPago;
                pagosToUpdate.Monto = pagos.Monto;
                pagosToUpdate.MetodoPago = pagos.MetodoPago;

                result = await base.Update(pagosToUpdate);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error al actualizar el pago";
                logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> Remove(Pagos pagos)
        {
            OperationResult result = new OperationResult();

            try
            {
                result = await base.Remove(pagos);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error al eliminar el Pago";
                logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> GetAll()
        {
            OperationResult result = new OperationResult();

            try
            {
                result.Data = await (from pagos in _realEstateContext.Pagos
                                     join contrato in _realEstateContext.Contratos on pagos.ContratoID equals contrato.ContratoID

                                     select new PagosModel()
                                     {
                                         PagoID = pagos.PagoID,
                                         ContratoID = contrato.ContratoID,
                                         FechaPago = pagos.FechaPago,
                                         Monto = pagos.Monto,
                                         MetodoPago = pagos.MetodoPago

                                     }).AsNoTracking()
                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error al obtener los pagos";
                logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }
        public async override Task<OperationResult> GetById(int id)
        {
            OperationResult result = new OperationResult();

            try
            {
                result.Data = await (from pagos in _realEstateContext.Pagos
                                     join contrato in _realEstateContext.Contratos on pagos.ContratoID equals contrato.ContratoID

                                     where pagos.PagoID == id

                                     select new PagosModel()
                                     {
                                         PagoID = pagos.PagoID,
                                         ContratoID = contrato.ContratoID,
                                         FechaPago = pagos.FechaPago,
                                         Monto = pagos.Monto,
                                         MetodoPago = pagos.MetodoPago

                                     }).AsNoTracking()
                                     .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error al obtener el pago";
                logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }
    }
}
