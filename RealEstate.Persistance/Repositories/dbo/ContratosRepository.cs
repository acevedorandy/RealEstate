

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
    public sealed class ContratosRepository(RealEstateContext realEstateContext, IdentityContext identityContext,
                                            ILogger<ContratosRepository> logger) : BaseRepository<Contratos>(realEstateContext), IContratosRepository
    {
        private readonly RealEstateContext _realEstateContext = realEstateContext;
        private readonly ILogger<ContratosRepository> _logger = logger;
        private readonly IdentityContext _identityContext = identityContext;

        public async override Task<OperationResult> Save(Contratos contratos)
        {
            OperationResult result = new OperationResult();

            try
            {
                result = await base.Save(contratos);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error guardando el contrato.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> Update(Contratos contratos)
        {
            OperationResult result = new OperationResult();

            try
            {
                Contratos? contratosToUpdate = await _realEstateContext.Contratos.FindAsync(contratos.ContratoID);

                contratosToUpdate.ContratoID = contratos.ContratoID;
                contratosToUpdate.PropiedadID = contratos.PropiedadID;
                contratosToUpdate.ClienteID = contratos.ClienteID;
                contratosToUpdate.AgenteID = contratos.AgenteID;
                contratosToUpdate.FechaInicio = contratos.FechaInicio;
                contratosToUpdate.FechaFin = contratos.FechaFin;
                contratosToUpdate.TipoContrato = contratos.TipoContrato;
                contratosToUpdate.Monto = contratos.Monto;

                result = await base.Update(contratosToUpdate);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error actualizando el contrato.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> Remove(Contratos contratos)
        {
            OperationResult result = new OperationResult();

            try
            {
                result = await base.Remove(contratos);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error eliminando el contrato.";
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

                var contratos = await _realEstateContext.Contratos
                    .ToListAsync();

                var propiedades = await _realEstateContext.Propiedades
                    .ToListAsync();

                var datos = (from contrato in contratos
                             join propiedad in propiedades on contrato.PropiedadID equals propiedad.PropiedadID
                             join agente in usuarios on contrato.AgenteID equals agente.Id
                             join cliente in usuarios on contrato.ClienteID equals cliente.Id

                             select new ContratosModel
                             {
                                 ContratoID = contrato.ContratoID,
                                 PropiedadID = propiedad.PropiedadID,
                                 ClienteID = cliente.Id,
                                 AgenteID = agente.Id,
                                 FechaInicio = contrato.FechaInicio,
                                 FechaFin = contrato.FechaInicio,
                                 TipoContrato = contrato.TipoContrato,
                                 Monto = contrato.Monto

                             }).ToList();

                result.Data = datos;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo los contratos";
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

                var contratos = await _realEstateContext.Contratos
                    .ToListAsync();

                var propiedades = await _realEstateContext.Propiedades
                    .ToListAsync();

                var datos = (from contrato in contratos
                             join propiedad in propiedades on contrato.PropiedadID equals propiedad.PropiedadID
                             join agente in usuarios on contrato.AgenteID equals agente.Id
                             join cliente in usuarios on contrato.ClienteID equals cliente.Id

                             where contrato.ContratoID == id

                             select new ContratosModel
                             {
                                 ContratoID = contrato.ContratoID,
                                 PropiedadID = propiedad.PropiedadID,
                                 ClienteID = cliente.Id,
                                 AgenteID = agente.Id,
                                 FechaInicio = contrato.FechaInicio,
                                 FechaFin = contrato.FechaInicio,
                                 TipoContrato = contrato.TipoContrato,
                                 Monto = contrato.Monto

                             }).FirstOrDefault();

                result.Data = datos;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo el contrato";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }
    }
}
