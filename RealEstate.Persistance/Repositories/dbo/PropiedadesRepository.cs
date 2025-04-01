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
    public sealed class PropiedadesRepository(RealEstateContext realEstateContext, ILogger<PropiedadesRepository> logger,
        PropiedadesValidate propiedadesValidate) : BaseRepository<Propiedades>(realEstateContext), IPropiedadesRepository
    {
        private readonly RealEstateContext realEstate_Context = realEstateContext;
        private readonly ILogger<PropiedadesRepository> logger = logger;
        private readonly PropiedadesValidate propiedades_Validate = propiedadesValidate;

        public async override Task<OperationResult> Save(Propiedades propiedades)
        {
            OperationResult result = new OperationResult();

            try
            {
                propiedades_Validate.PropiedadesValidations(result, propiedades);

                result = await base.Save(propiedades);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error al guardar la propiedad";
                logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }
        public async override Task<OperationResult> Update(Propiedades propiedades)
        {
            OperationResult result = new OperationResult();

            try
            {
                propiedades_Validate.PropiedadesValidations(result, propiedades);

                Propiedades? propiedadesToUpdate = await realEstate_Context.Propiedades.FindAsync(propiedades.PropiedadID);

                propiedadesToUpdate.Codigo = propiedades.Codigo;
                propiedadesToUpdate.AgenteID = propiedades.AgenteID;
                propiedadesToUpdate.Titulo = propiedades.Titulo;
                propiedadesToUpdate.Descripcion = propiedades.Descripcion;
                propiedadesToUpdate.Precio = propiedades.Precio;
                propiedadesToUpdate.Direccion = propiedades.Direccion;
                propiedadesToUpdate.Ciudad = propiedades.Ciudad;
                propiedadesToUpdate.Sector = propiedades.Sector;
                propiedadesToUpdate.CodigoPostal = propiedades.CodigoPostal;
                propiedadesToUpdate.TotalNivel = propiedades.TotalNivel;
                propiedadesToUpdate.Piso = propiedades.Piso;
                propiedadesToUpdate.AñoConstruccion = propiedades.AñoConstruccion;
                propiedadesToUpdate.TipoPropiedad = propiedades.TipoPropiedad;
                propiedadesToUpdate.Disponibilidad = propiedades.Disponibilidad;
                propiedadesToUpdate.Imagen = propiedades.Imagen;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error al actualizar la propiedad";
                logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }
        public async override Task<OperationResult> Remove(Propiedades propiedades)
        {
            OperationResult result = new OperationResult();

            try
            {
                if (propiedades == null)
                {
                    result.Success = false;
                    result.Message = "La entidad es requerida para esta accion";
                }

                result = await base.Remove(propiedades);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error al eliminar la propiedad";
                logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }
        public async override Task<OperationResult> GetAll()
        {
            OperationResult result = new OperationResult();

            try
            {
                result.Data = await (from propiedades in realEstate_Context.Propiedades
                                     select new PropiedadesModel()
                                     {
                                         PropiedadID = propiedades.PropiedadID,
                                         Codigo = propiedades.Codigo,
                                         AgenteID = propiedades.AgenteID,
                                         Titulo = propiedades.Titulo,
                                         Descripcion = propiedades.Descripcion,
                                         Precio = propiedades.Precio,
                                         Direccion = propiedades.Direccion,
                                         Ciudad = propiedades.Ciudad,
                                         Sector = propiedades.Sector,
                                         CodigoPostal = propiedades.CodigoPostal,
                                         TotalNivel = propiedades.TotalNivel,
                                         Piso = propiedades.Piso,
                                         AñoConstruccion = propiedades.AñoConstruccion,
                                         TipoPropiedad = propiedades.TipoPropiedad,
                                         Disponibilidad = propiedades.Disponibilidad,
                                         Imagen = propiedades.Imagen
                                     }).AsNoTracking()
                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error al obtener las propiedades";
                logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }
        public async override Task<OperationResult> GetById(int id)
        {
            OperationResult result = new OperationResult();

            try
            {
                result.Data = await (from propiedades in realEstate_Context.Propiedades
                                     where propiedades.PropiedadID == id
                                     select new PropiedadesModel()
                                     {
                                         PropiedadID = propiedades.PropiedadID,
                                         Codigo = propiedades.Codigo,
                                         AgenteID = propiedades.AgenteID,
                                         Titulo = propiedades.Titulo,
                                         Descripcion = propiedades.Descripcion,
                                         Precio = propiedades.Precio,
                                         Direccion = propiedades.Direccion,
                                         Ciudad = propiedades.Ciudad,
                                         Sector = propiedades.Sector,
                                         CodigoPostal = propiedades.CodigoPostal,
                                         TotalNivel = propiedades.TotalNivel,
                                         Piso = propiedades.Piso,
                                         AñoConstruccion = propiedades.AñoConstruccion,
                                         TipoPropiedad = propiedades.TipoPropiedad,
                                         Disponibilidad = propiedades.Disponibilidad,
                                         Imagen = propiedades.Imagen
                                     }).AsNoTracking()
                                     .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error al obtener la propiedad";
                logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }
    }
}
