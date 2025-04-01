using RealEstate.Domain.Entities.dbo;
using RealEstate.Domain.Result;

namespace RealEstate.Persistance.Validations
{
    public class PagosValidate
    {
        public OperationResult PagosValidations(OperationResult result, Pagos pagos)
        {
            OperationResult SetError(string message)
            {
                result.Success = false;
                result.Message = message;
                return result;
            }

            if (pagos == null)
                SetError("La entidad es requerida");
            if (pagos.ContratoID <= 0)
                SetError("El contrato es requerido");
            if (pagos.Monto <= 0)
                SetError("El monto es requerido");
            if (string.IsNullOrEmpty(pagos.MetodoPago))
                SetError("El metodo de pago es requerido");

            return result;
        }
    }
}
