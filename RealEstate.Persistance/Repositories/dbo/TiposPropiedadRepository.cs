

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
                result.Data = await (from tipoPropiedad in _realEstateContext.TiposPropiedad
                                     select new TiposPropiedadModel
                                     {
                                         TipoPropiedadID = tipoPropiedad.TipoPropiedadID,
                                         Nombre = tipoPropiedad.Nombre,
                                         Descripcion = tipoPropiedad.Descripcion,
                                         PropiedadesAsociadas = _realEstateContext.Propiedades
                                                               .Count(p => p.TipoPropiedad == tipoPropiedad.TipoPropiedadID)

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

        public async Task<OperationResult> RemoveTypeWithProperty(int tipoId)
        {
            OperationResult result = new OperationResult();

            using (var transaction = await _realEstateContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var propiedadIDs = await _realEstateContext.Propiedades
                        .Where(p => p.TipoPropiedad == tipoId)
                        .Select(p => p.PropiedadID)
                        .ToListAsync();

                    if (propiedadIDs.Any())
                    {
                        _realEstateContext.Favoritos.RemoveRange(
                            await _realEstateContext.Favoritos.Where(f => propiedadIDs.Contains(f.PropiedadID)).ToListAsync());

                        _realEstateContext.Mensajes.RemoveRange(
                            await _realEstateContext.Mensajes.Where(m => propiedadIDs.Contains(m.PropiedadID)).ToListAsync());

                        _realEstateContext.Ofertas.RemoveRange(
                            await _realEstateContext.Ofertas.Where(o => propiedadIDs.Contains(o.PropiedadID)).ToListAsync());

                        _realEstateContext.PropiedadFotos.RemoveRange(
                            await _realEstateContext.PropiedadFotos.Where(f => propiedadIDs.Contains(f.PropiedadID)).ToListAsync());

                        _realEstateContext.PropiedadMejoras.RemoveRange(
                            await _realEstateContext.PropiedadMejoras.Where(m => propiedadIDs.Contains(m.PropiedadID)).ToListAsync());

                        _realEstateContext.Propiedades.RemoveRange(
                            await _realEstateContext.Propiedades.Where(p => propiedadIDs.Contains(p.PropiedadID)).ToListAsync());
                    }

                    var propiedadTiposVenta = await _realEstateContext.PropiedadTiposVenta.FindAsync(tipoId);
                    if (propiedadTiposVenta != null)
                        _realEstateContext.Remove(propiedadTiposVenta);

                    var tipoPropiedad = await _realEstateContext.TiposPropiedad.FindAsync(tipoId);
                    if (tipoPropiedad != null)
                        _realEstateContext.TiposPropiedad.Remove(tipoPropiedad);

                    await _realEstateContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    result.Success = false;
                    result.Message = "Ha ocurrido un error eliminando el tipo de propiedad.";
                    _logger.LogError(ex, result.Message);
                }
                return result;
            }
        }
    }
}
