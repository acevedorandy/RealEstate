

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
    public class TiposPropiedadRepository(RealEstateContext realEstateContext,
                                          ILogger<TiposPropiedadRepository> logger) : BaseRepository<TiposPropiedad>(realEstateContext), ITiposPropiedadRepository
    {
        private readonly RealEstateContext _realEstateContext = realEstateContext;
        private readonly ILogger<TiposPropiedadRepository> _logger = logger;

        public async override Task<OperationResult> Save(TiposPropiedad tiposPropiedad)
        {
            OperationResult result = new OperationResult();

            try
            {
                result = await base.Save(tiposPropiedad);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error guardando el tipo de propiedad.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> Update(TiposPropiedad tiposPropiedad)
        {
            OperationResult result = new OperationResult();

            try
            {
                TiposPropiedad? tiposPropiedadToUpdate = await _realEstateContext.TiposPropiedad.FindAsync(tiposPropiedad.TipoPropiedadID);

                tiposPropiedadToUpdate.TipoPropiedadID = tiposPropiedad.TipoPropiedadID;
                tiposPropiedadToUpdate.Nombre = tiposPropiedad.Nombre;
                tiposPropiedadToUpdate.Descripcion = tiposPropiedad.Descripcion;

                result = await base.Update(tiposPropiedadToUpdate);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error actualizando el tipo de propiedad.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> Remove(TiposPropiedad tiposPropiedad)
        {
            OperationResult result = new OperationResult();

            try
            {
                result = await base.Remove(tiposPropiedad);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error eliminando el tipo de propiedad.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }
        public async override Task<OperationResult> GetAll()
        {
            OperationResult result = new OperationResult();

            try
            {
                result.Data = await (from tipo in _realEstateContext.TiposPropiedad
                                     
                                     select new TiposPropiedadModel
                                     {
                                         TipoPropiedadID = tipo.TipoPropiedadID,
                                         Nombre = tipo.Nombre,
                                         Descripcion = tipo.Descripcion

                                     }).AsNoTracking()
                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo los tipos de propiedades.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> GetById(int id)
        {
            OperationResult result = new OperationResult();

            try
            {
                result.Data = await (from tipo in _realEstateContext.TiposPropiedad

                                     where tipo.TipoPropiedadID == id

                                     select new TiposPropiedadModel
                                     {
                                         TipoPropiedadID = tipo.TipoPropiedadID,
                                         Nombre = tipo.Nombre,
                                         Descripcion = tipo.Descripcion

                                     }).AsNoTracking()
                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo el tipo de propiedad.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }
    }
}
