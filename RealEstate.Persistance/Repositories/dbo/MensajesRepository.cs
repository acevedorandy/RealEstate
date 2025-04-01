using Microsoft.Extensions.Logging;
using RealEstate.Domain.Entities.dbo;
using RealEstate.Domain.Result;
using RealEstate.Persistance.Base;
using RealEstate.Persistance.Context;
using RealEstate.Persistance.Interfaces.dbo;
using RealEstate.Persistance.Validations;

namespace RealEstate.Persistance.Repositories.dbo
{
    public sealed class MensajesRepository(RealEstateContext realEstateContext, ILogger<MensajesRepository> logger,
        MensajesValidate mensajesValidate) : BaseRepository<Mensajes>(realEstateContext), IMensajesRepository
    {
        private readonly RealEstateContext _realEstateContext = realEstateContext;
        private readonly MensajesValidate mensajes_Validate = mensajesValidate;
        private readonly ILogger<MensajesRepository> logger = logger;

        public async override Task<OperationResult> Save(Mensajes mensajes)
        {
            OperationResult result = new OperationResult();

            try
            {
                mensajes_Validate.MensajesValidations(result, mensajes);

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
        public async Task<OperationResult> Update(Mensajes mensajes)
        {
            OperationResult result = new OperationResult();

            try
            {

            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }
        public async Task<OperationResult> Remove(Mensajes mensajes)
        {
            OperationResult result = new OperationResult();

            try
            {

            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }
        public async Task<OperationResult> GetAll()
        {
            OperationResult result = new OperationResult();

            try
            {

            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }
        public async Task<OperationResult> GetById(int id)
        {
            OperationResult result = new OperationResult();

            try
            {

            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }
    }
}
