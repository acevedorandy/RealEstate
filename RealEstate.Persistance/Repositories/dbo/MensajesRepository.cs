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
    public sealed class MensajesRepository(RealEstateContext realEstateContext, IdentityContext identityContext, ILogger<MensajesRepository> logger,
        MensajesValidate mensajesValidate) : BaseRepository<Mensajes>(realEstateContext), IMensajesRepository
    {
        private readonly RealEstateContext _realEstateContext = realEstateContext;
        private readonly IdentityContext _identityContext = identityContext;
        private readonly MensajesValidate _mensajesValidate = mensajesValidate;
        private readonly ILogger<MensajesRepository> logger = logger;

        public async override Task<OperationResult> Save(Mensajes mensajes)
        {
            OperationResult result = new OperationResult();

            try
            {
                _mensajesValidate.MensajesValidations(result, mensajes);

                result = await base.Save(mensajes);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error al guardar el mensaje";
                logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> Update(Mensajes mensajes)
        {
            OperationResult result = new OperationResult();

            try
            {
                Mensajes? mensajesToUpdate = await _realEstateContext.Mensajes.FindAsync(mensajes.MensajeID);

                mensajesToUpdate.MensajeID = mensajes.MensajeID;
                mensajesToUpdate.RemitenteID = mensajes.RemitenteID;
                mensajesToUpdate.DestinatarioID = mensajes.DestinatarioID;
                mensajesToUpdate.PropiedadID = mensajes.PropiedadID;
                mensajesToUpdate.Mensaje = mensajes.Mensaje;
                mensajesToUpdate.Enviado = mensajes.Enviado;
                mensajesToUpdate.Visto = mensajes.Visto;

                result = await base.Update(mensajesToUpdate);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error actualizando el mensaje.";
                logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> Remove(Mensajes mensajes)
        {
            OperationResult result = new OperationResult();

            try
            {
                result = await base.Remove(mensajes);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error eliminando el mensaje.";
                logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> GetAll()
        {
            OperationResult result = new OperationResult();

            try
            {
                var remitentes = await _identityContext.Users
                    .ToListAsync();

                var destinatarios = await _identityContext.Users
                    .ToListAsync();

                var propiedades = await _realEstateContext.Propiedades
                    .ToListAsync();

                var mensajes = await _realEstateContext.Mensajes
                    .ToListAsync();

                var data = (from mensaje in mensajes
                            join remitente in remitentes on mensaje.RemitenteID equals remitente.Id
                            join destinatario in destinatarios on mensaje.DestinatarioID equals destinatario.Id
                            join propiedad in propiedades on mensaje.PropiedadID equals propiedad.PropiedadID

                            orderby mensaje.MensajeID descending

                            select new MensajesModel
                            {
                                MensajeID = mensaje.MensajeID,
                                RemitenteID = remitente.Id,
                                DestinatarioID = destinatario.Id,
                                PropiedadID = propiedad.PropiedadID,
                                Mensaje = mensaje.Mensaje,
                                Enviado = mensaje.Enviado,
                                Visto = mensaje.Visto,
                                RemitenteNombre = remitente.Nombre,
                                DestinatarioNombre = destinatario.Nombre

                            }).ToList();

                result.Data = data;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo los mensajes.";
                logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> GetById(int id)
        {
            OperationResult result = new OperationResult();

            try
            {
                var remitentes = await _identityContext.Users
                    .ToListAsync();

                var destinatarios = await _identityContext.Users
                    .ToListAsync();

                var propiedades = await _realEstateContext.Propiedades
                    .ToListAsync();

                var mensajes = await _realEstateContext.Mensajes
                    .ToListAsync();

                var data = (from mensaje in mensajes
                            join remitente in remitentes on mensaje.RemitenteID equals remitente.Id
                            join destinatario in destinatarios on mensaje.DestinatarioID equals destinatario.Id
                            join propiedad in propiedades on mensaje.PropiedadID equals propiedad.PropiedadID

                            select new MensajesModel
                            {
                                MensajeID = mensaje.MensajeID,
                                RemitenteID = remitente.Id,
                                DestinatarioID = destinatario.Id,
                                PropiedadID = propiedad.PropiedadID,
                                Mensaje = mensaje.Mensaje,
                                Enviado = mensaje.Enviado,
                                Visto = mensaje.Visto

                            }).FirstOrDefault();

                result.Data = data;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo el mensaje.";
                logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

public async Task<OperationResult> GetDestinatario(string remitenteId)
{
    OperationResult result = new OperationResult();

    try
    {
        // Obtener todos los mensajes donde el usuario actual participó (como remitente o destinatario)
        var userMessages = await _realEstateContext.Mensajes
            .Where(m => m.RemitenteID == remitenteId || m.DestinatarioID == remitenteId)
            .ToListAsync();

        // Agrupar por el "otro usuario" en la conversación y obtener el mensaje más reciente
        var latestMessages = userMessages
            .GroupBy(m => 
                m.RemitenteID == remitenteId ? m.DestinatarioID : m.RemitenteID) // Agrupa por el ID del otro usuario
            .Select(g => g.OrderByDescending(m => m.MensajeID).First()) // Toma el más reciente de cada grupo
            .ToList();

        // Obtener IDs de usuarios y propiedades involucradas
        var userIds = latestMessages
            .SelectMany(m => new[] { m.RemitenteID, m.DestinatarioID })
            .Distinct()
            .ToList();

        var propertyIds = latestMessages
            .Select(m => m.PropiedadID)
            .Distinct()
            .ToList();

        // Obtener datos relacionados
        var users = await _identityContext.Users
            .Where(u => userIds.Contains(u.Id))
            .ToDictionaryAsync(u => u.Id);

        var properties = await _realEstateContext.Propiedades
            .Where(p => propertyIds.Contains(p.PropiedadID))
            .ToDictionaryAsync(p => p.PropiedadID);

        // Mapear al modelo de vista
        var data = latestMessages.Select(m => new MensajesModel
        {
            MensajeID = m.MensajeID,
            RemitenteID = m.RemitenteID,
            RemitenteNombre = users.TryGetValue(m.RemitenteID, out var rem) ? rem.UserName : "Unknown",
            DestinatarioID = m.DestinatarioID,
            DestinatarioNombre = users.TryGetValue(m.DestinatarioID, out var des) ? des.UserName : "Unknown",
            PropiedadID = m.PropiedadID,
            Codigo = properties.TryGetValue(m.PropiedadID, out var prop) ? prop.Codigo : "N/A", // Aquí obtienes el código
            PropiedadNombre = properties.TryGetValue(m.PropiedadID, out var prop1) ? prop.Titulo : "Unknown",
            Mensaje = m.Mensaje,
            Enviado = m.Enviado,
            Visto = m.Visto,
            // Agregar campo para identificar si el usuario actual es el remitente
            EsRemitente = m.RemitenteID == remitenteId
        }).ToList();

        result.Data = data;
    }
    catch (Exception ex)
    {
        result.Success = false;
        result.Message = "Ha ocurrido un error obteniendo los mensajes.";
        logger.LogError(ex, result.Message);
    }

    return result;
}

    }
}
