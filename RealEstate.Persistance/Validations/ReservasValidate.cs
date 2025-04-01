using RealEstate.Domain.Entities.dbo;
using RealEstate.Domain.Result;

namespace RealEstate.Persistance.Validations
{
    public class ReservasValidate
    {
        public OperationResult ReservasValidations(OperationResult result, Reservas reservas)
        {
            OperationResult SetError(string message)
            {
                result.Success = false;
                result.Message = message;
                return result;
            }

            if (reservas == null)
                SetError("La entidad es requerida");
            if (reservas.PropiedadID <= 0)
                SetError("La propiedad es requerida");
            if (string.IsNullOrEmpty(reservas.ClienteID))
                SetError("El cliente es requerido");

            return result;
        }
    }
}
