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
    public class PropiedadFotosRepository(RealEstateContext realEstateContext,
                                          ILogger<PropiedadFotosRepository> logger) : BaseRepository<PropiedadFotos>(realEstateContext), IPropiedadFotosRepository
    {
        private readonly RealEstateContext _realEstateContext = realEstateContext;
        private readonly ILogger<PropiedadFotosRepository> _logger = logger;

        public async override Task<OperationResult> Save(PropiedadFotos propiedadFotos)
        {
            OperationResult result = new OperationResult();

            try
            {
                result = await base.Save(propiedadFotos);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error guardando la relacion.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> Update(PropiedadFotos propiedadFotos)
        {
            OperationResult result = new OperationResult();

            try
            {
                PropiedadFotos? propiedadFotosToUpdate = await _realEstateContext.PropiedadFotos.FindAsync(propiedadFotos.RelacionID);

                propiedadFotosToUpdate.RelacionID = propiedadFotos.RelacionID;
                propiedadFotosToUpdate.PropiedadID = propiedadFotos.PropiedadID;
                propiedadFotosToUpdate.Foto = propiedadFotos.Foto;

                result = await base.Update(propiedadFotosToUpdate);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error actualizando la relacion.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> Remove(PropiedadFotos propiedadFotos)
        {
            OperationResult result = new OperationResult();

            try
            {
                result = await base.Remove(propiedadFotos);
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
                result.Data = await (from propiedad in _realEstateContext.PropiedadFotos

                                     select new PropiedadFotosModel
                                     {
                                         RelacionID = propiedad.RelacionID,
                                         PropiedadID = propiedad.PropiedadID,
                                         Foto = propiedad.Foto

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
                result.Data = await (from propiedad in _realEstateContext.PropiedadFotos

                                     where propiedad.RelacionID == id

                                     select new PropiedadFotosModel 
                                     {
                                         RelacionID = propiedad.RelacionID,
                                         PropiedadID = propiedad.PropiedadID,
                                         Foto = propiedad.Foto

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
