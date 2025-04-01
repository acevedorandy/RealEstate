using RealEstate.Domain.Entities.dbo;
using RealEstate.Domain.Result;

namespace RealEstate.Persistance.Validations
{
    public class MensajesValidate
    {
        public OperationResult MensajesValidations(OperationResult result, Mensajes mensajes)
        {
            OperationResult SetError(string message)
            {
                result.Success = false;
                result.Message = message;
                return result;
            }

            if (mensajes == null)
                SetError("La entidad es reqquerida");
            if (string.IsNullOrEmpty(mensajes.RemitenteID))
                SetError("El remitente es requrido");
            if (string.IsNullOrEmpty(mensajes.DestinatarioID))
                SetError("El remitente es destinatario");
            if (mensajes.PropiedadID <= 0)
                SetError("La propiedad es requerida");
            if (string.IsNullOrEmpty(mensajes.Mensaje))
                SetError("El remitente es mensaje");

            return result;
        }
    }
}
