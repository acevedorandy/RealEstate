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
    public sealed class ReservasRepository(RealEstateContext realEstateContext, ILogger<ReservasRepository> logger,
        ReservasValidate reservasValidate) : BaseRepository<Reservas>(realEstateContext), IReservasRepository
    {
        private readonly RealEstateContext realEstate_Context = realEstateContext;
        private readonly ILogger<ReservasRepository> logger = logger;
        private readonly ReservasValidate reservas_Validate = reservasValidate;

        public async override Task<OperationResult> Save(Reservas reservas)
        {
            OperationResult result = new OperationResult();

            try
            {
                reservas_Validate.ReservasValidations(result, reservas);

                result = await base.Save(reservas);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error al guardar la reserva";
                logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }
        public async override Task<OperationResult> Update(Reservas reservas)
        {
            OperationResult result = new OperationResult();

            try
            {
                reservas_Validate.ReservasValidations(result, reservas);

                Reservas? reservasToUpdate = await realEstate_Context.Reservas.FindAsync(reservas.ReservaID);
                
                reservasToUpdate.PropiedadID = reservas.PropiedadID;
                reservasToUpdate.ClienteID = reservas.ClienteID;
                reservasToUpdate.FechaHora = reservas.FechaHora;
                reservasToUpdate.Estado = reservas.Estado;

                result = await base.Update(reservasToUpdate);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error al actualizar la reserva";
                logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }
        public async override Task<OperationResult> Remove(Reservas reservas)
        {
            OperationResult result = new OperationResult();

            try
            {
                if(reservas == null)
                {
                    result.Success = false;
                    result.Message = "La entidad es requerida para esta funcion";
                    return result;
                }

                result = await base.Remove(reservas);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error al eliminar la reserva";
                logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }
        public async override Task<OperationResult> GetAll()
        {
            OperationResult result = new OperationResult();

            try
            {
                result.Data = await (from reserva in realEstate_Context.Reservas
                                     join propiedad in realEstate_Context.Propiedades on reserva.PropiedadID equals propiedad.PropiedadID
                                     orderby reserva.ReservaID ascending
                                     /*join cliente in realEstate_Context.Usuarios on reserva.ClienteID equals cliente.UserId*/
                                     select new ReservasModel()
                                     {
                                         ReservaID = reserva.ReservaID,
                                         PropiedadID = propiedad.PropiedadID,
                                         //ClienteID = cliente.UserId,
                                         FechaHora = reserva.FechaHora,
                                         Estado = reserva.Estado
                                     }).AsNoTracking()
                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error al obtener las reservas";
                logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }
        public async override Task<OperationResult> GetById(int id)
        {
            OperationResult result = new OperationResult();

            try
            {
                result.Data = await (from reserva in realEstate_Context.Reservas
                                     join propiedad in realEstate_Context.Propiedades on reserva.PropiedadID equals propiedad.PropiedadID
                                     /*join cliente in realEstate_Context.Usuarios on reserva.ClienteID equals cliente.UserId*/
                                     where reserva.ReservaID == id
                                     select new ReservasModel()
                                     {
                                         ReservaID = reserva.ReservaID,
                                         PropiedadID = propiedad.PropiedadID,
                                         //ClienteID = cliente.UserId,
                                         FechaHora = reserva.FechaHora,
                                         Estado = reserva.Estado
                                     }).AsNoTracking()
                                     .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error al obtener la reserva";
                logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }
    }
}
