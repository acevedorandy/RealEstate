using RealEstate.Domain.Entities.dbo;
using RealEstate.Domain.Result;

namespace RealEstate.Persistance.Validations
{
    public class PropiedadesValidate
    {
        public OperationResult PropiedadesValidations(OperationResult result, Propiedades propiedades)
        {
            OperationResult SetError(string message)
            {
                result.Success = false;
                result.Data = message;
                return result;
            }

            if (propiedades == null)
                SetError("La entidad es requerida");
            if (string.IsNullOrEmpty(propiedades.Codigo))
                SetError("El codigo es requerido");
            if (string.IsNullOrEmpty(propiedades.AgenteID))
                SetError("El agente es requerido");
            if (string.IsNullOrEmpty(propiedades.Titulo))
                SetError("El titulo es requerido");
            if (string.IsNullOrEmpty(propiedades.Descripcion))
                SetError("La descripcion es requerida");
            if (propiedades.Precio <= 0)
                SetError("El precio es requerido");
            if (string.IsNullOrEmpty(propiedades.Direccion))
                SetError("La direccion es requerida");
            if (string.IsNullOrEmpty(propiedades.Ciudad))
                SetError("La ciudad es requerida");
            if (string.IsNullOrEmpty(propiedades.Sector))
                SetError("El sector es requerida");
            if (string.IsNullOrEmpty(propiedades.CodigoPostal))
                SetError("El codigo postal es requerido");
            if (propiedades.TotalNivel <= 0)
                SetError("El nivel total es requerido");
            if (propiedades.Piso <= 0)
                SetError("El piso es requerido");
            if (propiedades.TipoPropiedad <= 0)
                SetError("El tipo de propiedad es requerido");

            return result;
        }
    }
}
