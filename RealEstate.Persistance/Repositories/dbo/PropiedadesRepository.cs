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

                propiedadesToUpdate.Habitaciones = propiedades.Habitaciones;
                propiedadesToUpdate.Baños = propiedades.Baños;
                propiedadesToUpdate.Parqueos = propiedades.Parqueos;
                propiedadesToUpdate.TamañoTerreno = propiedades.TamañoTerreno;

                propiedadesToUpdate.TotalNivel = propiedades.TotalNivel;
                propiedadesToUpdate.Piso = propiedades.Piso;
                propiedadesToUpdate.AñoConstruccion = propiedades.AñoConstruccion;
                propiedadesToUpdate.TipoPropiedad = propiedades.TipoPropiedad;
                propiedadesToUpdate.Disponibilidad = propiedades.Disponibilidad;
                propiedadesToUpdate.Imagen = propiedades.Imagen;

                if (propiedades.Vendida != null)
                {
                    propiedadesToUpdate.Vendida = propiedades.Vendida;
                }
                propiedadesToUpdate.TipoVenta = propiedades.TipoVenta;

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

                var tipoVentas = await _realEstateContext.TiposVenta 
                    .ToListAsync();

                var datos = (from propiedad in propiedades
                             join agente in usuarios on propiedad.AgenteID equals agente.Id
                             join ventas in tipoVentas on propiedad.TipoVenta equals ventas.TipoVentaID

                             orderby propiedad.PropiedadID descending

                             //where propiedad.Vendida == false || propiedad.Vendida == null

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
                                    Habitaciones = propiedad.Habitaciones,
                                    Baños = propiedad.Baños,
                                    Parqueos = propiedad.Parqueos,
                                    TamañoTerreno = propiedad.TamañoTerreno,
                                    TotalNivel = propiedad.TotalNivel,
                                    Piso = propiedad.Piso,
                                    AñoConstruccion = propiedad.AñoConstruccion,
                                    TipoPropiedad = propiedad.TipoPropiedad,
                                    Disponibilidad = propiedad.Disponibilidad,
                                    Imagen = propiedad != null ? propiedad.Imagen : (string?)null,
                                    Vendida = propiedad.Vendida,
                                    TipoVenta = ventas.TipoVentaID,

                                }).ToList();

                result.Data = datos;
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

                var tipoVentas = await _realEstateContext.TiposVenta
                    .ToListAsync();

                var datos = (from propiedad in propiedades
                             join agente in usuarios on propiedad.AgenteID equals agente.Id
                             join ventas in tipoVentas on propiedad.TipoVenta equals ventas.TipoVentaID

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
                                 Habitaciones = propiedad.Habitaciones,
                                 Baños = propiedad.Baños,
                                 Parqueos = propiedad.Parqueos,
                                 TamañoTerreno = propiedad.TamañoTerreno,
                                 TotalNivel = propiedad.TotalNivel,
                                 Piso = propiedad.Piso,
                                 AñoConstruccion = propiedad.AñoConstruccion,
                                 TipoPropiedad = propiedad.TipoPropiedad,
                                 Disponibilidad = propiedad.Disponibilidad,
                                 Imagen = propiedad.Imagen,
                                 TipoVenta = ventas.TipoVentaID,

                             }).FirstOrDefault();

                result.Data = datos;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo la propiedad";
                logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async Task<OperationResult> GetAgentByProperty(int propiedadId)
        {
            OperationResult result = new OperationResult();

            try
            {
                var propiedades = await _realEstateContext.Propiedades
                    .ToListAsync();

                var usuarios = await _identityContext.Users
                    .ToListAsync();

                var datos = (from propiedad in propiedades
                             join user in usuarios on propiedad.AgenteID equals user.Id

                             where propiedad.PropiedadID == propiedadId

                             select new UsuariosModel
                             {
                                 Id = user.Id,
                                 Nombre = user.Nombre,
                                 Apellido = user.Apellido,
                                 Foto = user.Foto,
                                 Email = user.Email,
                                 Telefono = user.PhoneNumber

                             }).FirstOrDefault();

                result.Data = datos;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo la propiedad";
                logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async Task<OperationResult> GetAllPropertyByAgent(string agenteId)
        {
            OperationResult result = new OperationResult();

            try
            {
                var propiedades = await _realEstateContext.Propiedades
                    .ToListAsync();

                var usuarios = await _identityContext.Users
                    .ToListAsync();

                var tipoVentas = await _realEstateContext.TiposVenta
                    .ToListAsync();

                var datos = (from propiedad in propiedades
                             join agente in usuarios on propiedad.AgenteID equals agente.Id
                             join ventas in tipoVentas on propiedad.TipoVenta equals ventas.TipoVentaID

                             orderby propiedad.PropiedadID descending

                             where propiedad.AgenteID == agenteId /*&&
                                   (propiedad.Vendida == false || propiedad.Vendida == null)*/

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
                                 Habitaciones = propiedad.Habitaciones,
                                 Baños = propiedad.Baños,
                                 Parqueos = propiedad.Parqueos,
                                 TamañoTerreno = propiedad.TamañoTerreno,
                                 TotalNivel = propiedad.TotalNivel,
                                 Piso = propiedad.Piso,
                                 AñoConstruccion = propiedad.AñoConstruccion,
                                 TipoPropiedad = propiedad.TipoPropiedad,
                                 Disponibilidad = propiedad.Disponibilidad,
                                 Imagen = propiedad.Imagen,
                                 Vendida = propiedad.Vendida,
                                 TipoVenta = ventas.TipoVentaID,

                             }).ToList();

                result.Data = datos;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo la propiedad";
                logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async Task<OperationResult> MarkAsSold(int propiedadId)
        {
            OperationResult result = new OperationResult();

            try
            {
                Propiedades? propiedadAsSold = await _realEstateContext.Propiedades.FindAsync(propiedadId);

                propiedadAsSold.Disponibilidad = false;
                propiedadAsSold.Vendida = true;

                result = await base.Update(propiedadAsSold);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error marcando la propiedad como vendida";
                logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async Task<OperationResult> RemovePropertyWithAll(int propiedadId)
        {
            OperationResult result = new OperationResult();

            using (var transaction = await _realEstateContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var propiedadIDs = await _realEstateContext.Propiedades
                        .Where(p => p.PropiedadID == propiedadId)
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

                        _realEstateContext.PropiedadTiposVenta.RemoveRange(
                            await _realEstateContext.PropiedadTiposVenta.Where(v => propiedadIDs.Contains(v.TipoVentaID)).ToListAsync());
                    }

                    var propiedad = await _realEstateContext.Propiedades.FindAsync(propiedadId);
                    if (propiedad != null)
                        _realEstateContext.Propiedades.Remove(propiedad);

                    await _realEstateContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    result.Success = false;
                    result.Message = "Ha ocurrido un error eliminando la propiedad y todas sus relaciones.";
                    logger.LogError(ex, result.Message);
                }
                return result;
            }
        }

        public async Task<OperationResult> GetPropertyByCode(string codigo)
        {
            OperationResult result = new OperationResult();

            try
            {
                var propiedades = await _realEstateContext.Propiedades
                    .ToListAsync();

                var usuarios = await _identityContext.Users
                    .ToListAsync();

                var tipoVentas = await _realEstateContext.TiposVenta
                    .ToListAsync();

                var datos = (from propiedad in propiedades
                             join agente in usuarios on propiedad.AgenteID equals agente.Id
                             join ventas in tipoVentas on propiedad.TipoVenta equals ventas.TipoVentaID

                             where propiedad.Codigo == codigo

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
                                 Habitaciones = propiedad.Habitaciones,
                                 Baños = propiedad.Baños,
                                 Parqueos = propiedad.Parqueos,
                                 TamañoTerreno = propiedad.TamañoTerreno,
                                 TotalNivel = propiedad.TotalNivel,
                                 Piso = propiedad.Piso,
                                 AñoConstruccion = propiedad.AñoConstruccion,
                                 TipoPropiedad = propiedad.TipoPropiedad,
                                 Disponibilidad = propiedad.Disponibilidad,
                                 Imagen = propiedad.Imagen,
                                 TipoVenta = ventas.TipoVentaID,

                             }).FirstOrDefault();

                result.Data = datos;
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
