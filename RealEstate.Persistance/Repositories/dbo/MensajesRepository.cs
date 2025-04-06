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
                                Visto = mensaje.Visto

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
    }
}
