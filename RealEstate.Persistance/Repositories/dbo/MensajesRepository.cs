using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RealEstate.Domain.Entities.dbo;
using RealEstate.Domain.Result;
using RealEstate.Identity.Shared.Context;
using RealEstate.Persistance.Base;
using RealEstate.Persistance.Context;
using RealEstate.Persistance.Interfaces.dbo;
using RealEstate.Persistance.Models.dbo;
using RealEstate.Persistance.Models.ViewModel;
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
                var userMessages = await _realEstateContext.Mensajes
                    .Where(m => m.RemitenteID == remitenteId || m.DestinatarioID == remitenteId)
                    .ToListAsync();

                var latestMessages = userMessages
                    .GroupBy(m =>
                        m.RemitenteID == remitenteId ? m.DestinatarioID : m.RemitenteID)
                    .Select(g => g.OrderByDescending(m => m.MensajeID).First())
                    .ToList();

                var userIds = latestMessages
                    .SelectMany(m => new[] { m.RemitenteID, m.DestinatarioID })
                    .Distinct()
                    .ToList();

                var propertyIds = latestMessages
                    .Select(m => m.PropiedadID)
                    .Distinct()
                    .ToList();

                var users = await _identityContext.Users
                    .Where(u => userIds.Contains(u.Id))
                    .ToDictionaryAsync(u => u.Id);

                var properties = await _realEstateContext.Propiedades
                    .Where(p => propertyIds.Contains(p.PropiedadID))
                    .ToDictionaryAsync(p => p.PropiedadID);

                var data = latestMessages.Select(m => new MensajesModel
                {
                    MensajeID = m.MensajeID,
                    RemitenteID = m.RemitenteID,
                    RemitenteNombre = users.TryGetValue(m.RemitenteID, out var rem) ? rem.UserName : "Unknown",
                    DestinatarioID = m.DestinatarioID,
                    DestinatarioNombre = users.TryGetValue(m.DestinatarioID, out var des) ? des.UserName : "Unknown",
                    PropiedadID = m.PropiedadID,
                    Codigo = properties.TryGetValue(m.PropiedadID, out var prop) ? prop.Codigo : "N/A",
                    PropiedadNombre = properties.TryGetValue(m.PropiedadID, out var prop1) ? prop.Titulo : "Unknown",
                    Mensaje = m.Mensaje,
                    Enviado = m.Enviado,
                    Visto = m.Visto,

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

        public async Task<OperationResult> GetConversationBetween(string remitenteId, string destinatarioId)
        {
            OperationResult result = new OperationResult();

            try
            {
                var remitentes = await _identityContext.Users.ToListAsync();
                var destinatarios = remitentes;

                var propiedades = await _realEstateContext.Propiedades.ToListAsync();
                var mensajes = await _realEstateContext.Mensajes.ToListAsync();

                var data = (from mensaje in mensajes
                            join remitente in remitentes on mensaje.RemitenteID equals remitente.Id
                            join destinatario in destinatarios on mensaje.DestinatarioID equals destinatario.Id
                            join propiedad in propiedades on mensaje.PropiedadID equals propiedad.PropiedadID into propiedadJoin
                            from propiedad in propiedadJoin.DefaultIfEmpty()

                            where
                                (mensaje.RemitenteID == remitenteId && mensaje.DestinatarioID == destinatarioId)
                                || (mensaje.RemitenteID == destinatarioId && mensaje.DestinatarioID == remitenteId)

                            orderby mensaje.MensajeID descending

                            select new MensajesModel
                            {
                                MensajeID = mensaje.MensajeID,
                                RemitenteID = remitente.Id,
                                DestinatarioID = destinatario.Id,
                                PropiedadID = mensaje.PropiedadID,
                                Mensaje = mensaje.Mensaje,
                                Enviado = mensaje.Enviado,
                                Visto = mensaje.Visto,
                                RemitenteNombre = remitente.Nombre,
                                DestinatarioNombre = destinatario.Nombre,
                                PropiedadNombre = propiedad?.Titulo
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

        public async Task<OperationResult> GetChatsByClient(string clienteId)
        {
            OperationResult result = new OperationResult();

            try
            {
                // Paso 1: Obtener los últimos mensajes por propiedad
                var mensajesRecientes = await _realEstateContext.Mensajes
                    .Where(m => m.RemitenteID == clienteId)
                    .GroupBy(m => m.PropiedadID)
                    .Select(g => g.OrderByDescending(m => m.Enviado).FirstOrDefault())
                    .ToListAsync();

                var remitenteIds = mensajesRecientes.Select(m => m.RemitenteID).Distinct().ToList();
                var destinatarioIds = mensajesRecientes.Select(m => m.DestinatarioID).Distinct().ToList();
                var propiedadIds = mensajesRecientes.Select(m => m.PropiedadID).Distinct().ToList();

                // Paso 2: Obtener usuarios y propiedades por separado
                var remitentes = await _identityContext.Users
                    .Where(u => remitenteIds.Contains(u.Id))
                    .ToListAsync();

                var destinatarios = await _identityContext.Users
                    .Where(u => destinatarioIds.Contains(u.Id))
                    .ToListAsync();

                var propiedades = await _realEstateContext.Propiedades
                    .Where(p => propiedadIds.Contains(p.PropiedadID))
                    .ToListAsync();

                // Paso 3: Juntar los datos en memoria
                var viewModels = mensajesRecientes.Select(m =>
                {
                    var remitente = remitentes.FirstOrDefault(u => u.Id == m.RemitenteID);
                    var destinatario = destinatarios.FirstOrDefault(u => u.Id == m.DestinatarioID);
                    var propiedad = propiedades.FirstOrDefault(p => p.PropiedadID == m.PropiedadID);

                    return new MensajesViewModel
                    {
                        MensajeID = m.MensajeID,
                        RemitenteID = remitente?.Id,
                        DestinatarioID = destinatario?.Id,
                        PropiedadID = propiedad.PropiedadID,
                        Mensaje = m.Mensaje,
                        Enviado = m.Enviado,
                        Visto = m.Visto,
                        RemitenteNombre = remitente?.Nombre,
                        DestinatarioNombre = destinatario?.Nombre,
                        PropiedadNombre = propiedad?.Titulo,
                        PropiedadFoto = propiedad?.Imagen
                    };
                }).ToList();

                result.Data = viewModels;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo los mensajes.";
                logger.LogError(result.Message, ex.ToString());
            }

            return result;
        }

        public async Task<OperationResult> GetChatsByAgent(string agenteId)
        {
            OperationResult result = new OperationResult();

            try
            {
                // Paso 1: Obtener el mensaje más reciente de cada remitente hacia este destinatario
                var mensajesRecientes = await _realEstateContext.Mensajes
                    .Where(m => m.DestinatarioID == agenteId)
                    .GroupBy(m => m.RemitenteID)
                    .Select(g => g.OrderByDescending(m => m.Enviado).FirstOrDefault())
                    .ToListAsync();

                var remitenteIds = mensajesRecientes.Select(m => m.RemitenteID).Distinct().ToList();
                var propiedadIds = mensajesRecientes.Select(m => m.PropiedadID).Distinct().ToList();

                var remitentes = await _identityContext.Users
                    .Where(u => remitenteIds.Contains(u.Id))
                    .ToListAsync();

                var destinatario = await _identityContext.Users
                    .FirstOrDefaultAsync(u => u.Id == agenteId);

                var propiedades = await _realEstateContext.Propiedades
                    .Where(p => propiedadIds.Contains(p.PropiedadID))
                    .ToListAsync();

                var viewModels = mensajesRecientes.Select(m =>
                {
                    var remitente = remitentes.FirstOrDefault(u => u.Id == m.RemitenteID);
                    var propiedad = propiedades.FirstOrDefault(p => p.PropiedadID == m.PropiedadID);

                    return new MensajesViewModel
                    {
                        MensajeID = m.MensajeID,
                        RemitenteID = remitente?.Id,
                        DestinatarioID = destinatario?.Id,
                        PropiedadID = propiedad?.PropiedadID ?? 0,
                        Mensaje = m.Mensaje,
                        Enviado = m.Enviado,
                        Visto = m.Visto,
                        RemitenteNombre = remitente?.Nombre,
                        DestinatarioNombre = destinatario?.Nombre,
                        PropiedadNombre = propiedad?.Titulo,
                        PropiedadFoto = propiedad?.Imagen
                    };
                }).ToList();

                result.Data = viewModels;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo los mensajes.";
                logger.LogError(result.Message, ex.ToString());
            }

            return result;
        }

        public async Task<OperationResult> GetConversation(int propiedadId, string destinatarioId, string remitenteId)
        {
            OperationResult result = new OperationResult();

            try
            {
                // Paso 1: Obtener los mensajes relevantes de esa conversación
                var mensajes = await _realEstateContext.Mensajes
                    .Where(m =>
                        m.PropiedadID == propiedadId &&
                        (
                            (m.RemitenteID == remitenteId && m.DestinatarioID == destinatarioId) ||
                            (m.RemitenteID == destinatarioId && m.DestinatarioID == remitenteId)
                        )
                    )
                    .OrderByDescending(m => m.MensajeID)
                    .ToListAsync();

                // Paso 2: Obtener los IDs únicos necesarios
                var usuarioIds = mensajes
                    .SelectMany(m => new[] { m.RemitenteID, m.DestinatarioID })
                    .Distinct()
                    .ToList();

                var propiedadIds = mensajes
                    .Select(m => m.PropiedadID)
                    .Distinct()
                    .ToList();

                // Paso 3: Consultar usuarios y propiedades desde sus respectivos contextos
                var usuarios = await _identityContext.Users
                    .Where(u => usuarioIds.Contains(u.Id))
                    .ToListAsync();

                var propiedades = await _realEstateContext.Propiedades
                    .Where(p => propiedadIds.Contains(p.PropiedadID))
                    .ToListAsync();

                // Paso 4: Armar el resultado en memoria
                var data = mensajes.Select(m =>
                {
                    var remitente = usuarios.FirstOrDefault(u => u.Id == m.RemitenteID);
                    var destinatario = usuarios.FirstOrDefault(u => u.Id == m.DestinatarioID);
                    var propiedad = propiedades.FirstOrDefault(p => p.PropiedadID == m.PropiedadID);

                    return new MensajesViewModel
                    {
                        MensajeID = m.MensajeID,
                        RemitenteID = m.RemitenteID,
                        DestinatarioID = m.DestinatarioID,
                        PropiedadID = m.PropiedadID,
                        Mensaje = m.Mensaje,
                        Enviado = m.Enviado,
                        Visto = m.Visto,
                        RemitenteNombre = remitente?.Nombre,
                        DestinatarioNombre = destinatario?.Nombre,
                        PropiedadNombre = propiedad?.Titulo,
                        PropiedadFoto = propiedad.Imagen
                    };
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

    }
}
