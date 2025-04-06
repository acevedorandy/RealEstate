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
    public sealed class PropiedadesRepository(RealEstateContext realEstateContext, IdentityContext identityContext, ILogger<PropiedadesRepository> logger,
        PropiedadesValidate propiedadesValidate) : BaseRepository<Propiedades>(realEstateContext), IPropiedadesRepository
    {
        private readonly RealEstateContext _realEstateContext = realEstateContext;
        private readonly IdentityContext _identityContext = identityContext;
        private readonly ILogger<PropiedadesRepository> logger = logger;
        private readonly PropiedadesValidate _propiedadesValidate = propiedadesValidate;

        public async override Task<OperationResult> Save(Propiedades propiedades)
        {
            OperationResult result = new OperationResult();

            try
            {
                _propiedadesValidate.PropiedadesValidations(result, propiedades);

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
                _propiedadesValidate.PropiedadesValidations(result, propiedades);

                Propiedades? propiedadesToUpdate = await _realEstateContext.Propiedades.FindAsync(propiedades.PropiedadID);

                propiedadesToUpdate.PropiedadID = propiedades.PropiedadID;
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

                result = await base.Update(propiedadesToUpdate);
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
                var propiedades = await _realEstateContext.Propiedades
                    .ToListAsync();

                var usuarios = await _identityContext.Users
                    .ToListAsync();

                var datos = (from propiedad in propiedades
                             join agente in usuarios on propiedad.AgenteID equals agente.Id

                                select new PropiedadesModel()
                                {
                                    PropiedadID = propiedad.PropiedadID,
                                    Codigo = propiedad.Codigo,
                                    AgenteID = agente.Id,
                                    Titulo = propiedad.Titulo,
                                    Descripcion = propiedad.Descripcion,
                                    Precio = propiedad.Precio,
                                    Direccion = propiedad.Direccion,
                                    Ciudad = propiedad.Ciudad,
                                    Sector = propiedad.Sector,
                                    CodigoPostal = propiedad.CodigoPostal,
                                    TotalNivel = propiedad.TotalNivel,
                                    Piso = propiedad.Piso,
                                    AñoConstruccion = propiedad.AñoConstruccion,
                                    TipoPropiedad = propiedad.TipoPropiedad,
                                    Disponibilidad = propiedad.Disponibilidad,
                                    Imagen = propiedad.Imagen

                                }).ToList();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo las propiedades";
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

                var datos = (from propiedad in propiedades
                             join agente in usuarios on propiedad.AgenteID equals agente.Id

                             where propiedad.PropiedadID == id

                             select new PropiedadesModel()
                             {
                                 PropiedadID = propiedad.PropiedadID,
                                 Codigo = propiedad.Codigo,
                                 AgenteID = agente.Id,
                                 Titulo = propiedad.Titulo,
                                 Descripcion = propiedad.Descripcion,
                                 Precio = propiedad.Precio,
                                 Direccion = propiedad.Direccion,
                                 Ciudad = propiedad.Ciudad,
                                 Sector = propiedad.Sector,
                                 CodigoPostal = propiedad.CodigoPostal,
                                 TotalNivel = propiedad.TotalNivel,
                                 Piso = propiedad.Piso,
                                 AñoConstruccion = propiedad.AñoConstruccion,
                                 TipoPropiedad = propiedad.TipoPropiedad,
                                 Disponibilidad = propiedad.Disponibilidad,
                                 Imagen = propiedad.Imagen

                             }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo la propiedad";
                logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }
    }
}
