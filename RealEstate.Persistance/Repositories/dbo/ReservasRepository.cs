using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RealEstate.Domain.Entities.dbo;
using RealEstate.Domain.Result;
using RealEstate.Identity.Shared.Context;
using RealEstate.Persistance.Base;
using RealEstate.Persistance.Context;
using RealEstate.Persistance.Interfaces.dbo;
using RealEstate.Persistance.Models.dbo;
using RealEstate.Persistance.Validations;

namespace RealEstate.Persistance.Repositories.dbo
{
    public sealed class ReservasRepository(RealEstateContext realEstateContext, IdentityContext identityContext, ILogger<ReservasRepository> logger,
        ReservasValidate reservasValidate) : BaseRepository<Reservas>(realEstateContext), IReservasRepository
    {
        private readonly RealEstateContext _realEstateContext = realEstateContext;
        private readonly IdentityContext _identityContext = identityContext;
        private readonly ILogger<ReservasRepository> logger = logger;
        private readonly ReservasValidate _reservasValidate = reservasValidate;

        public async override Task<OperationResult> Save(Reservas reservas)
        {
            OperationResult result = new OperationResult();

            try
            {
                _reservasValidate.ReservasValidations(result, reservas);

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
                _reservasValidate.ReservasValidations(result, reservas);

                Reservas? reservasToUpdate = await _realEstateContext.Reservas.FindAsync(reservas.ReservaID);
                
                reservasToUpdate.ReservaID = reservas.ReservaID;
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
                var propiedades = await _realEstateContext.Propiedades
                    .ToListAsync();

                var usuarios = await _identityContext.Users
                    .ToListAsync();

                var reservas = await _realEstateContext.Reservas 
                    .ToListAsync();

                var datos = (from reserva in reservas
                             join propiedad in propiedades on reserva.PropiedadID equals propiedad.PropiedadID
                             join cliente in usuarios on reserva.ClienteID equals cliente.Id

                             orderby reserva.ReservaID ascending
                             select new ReservasModel()
                             {
                                 ReservaID = reserva.ReservaID,
                                 PropiedadID = propiedad.PropiedadID,
                                 ClienteID = cliente.Id,
                                 FechaHora = reserva.FechaHora,
                                 Estado = reserva.Estado

                             }).ToList();

                result.Data = datos;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo las reservas";
                logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> GetById(int id)
        {
            OperationResult result = new OperationResult();

            try
            {
                var propiedades = await _realEstateContext.Propiedades
                    .ToListAsync();

                var usuarios = await _identityContext.Users
                    .ToListAsync();

                var reservas = await _realEstateContext.Reservas
                    .ToListAsync();

                var datos = (from reserva in reservas
                             join propiedad in propiedades on reserva.PropiedadID equals propiedad.PropiedadID
                             join cliente in usuarios on reserva.ClienteID equals cliente.Id

                             where reserva.ReservaID == id

                             select new ReservasModel()
                             {
                                 ReservaID = reserva.ReservaID,
                                 PropiedadID = propiedad.PropiedadID,
                                 ClienteID = cliente.Id,
                                 FechaHora = reserva.FechaHora,
                                 Estado = reserva.Estado

                             }).ToList();

                result.Data = datos;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo las reservas";
                logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }
    }
}
