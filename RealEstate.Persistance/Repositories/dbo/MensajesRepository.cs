

using Microsoft.Extensions.Logging;
using RealEstate.Domain.Entities.dbo;
using RealEstate.Domain.Result;
using RealEstate.Persistance.Base;
using RealEstate.Persistance.Context;
using RealEstate.Persistance.Interfaces.dbo;

namespace RealEstate.Persistance.Repositories.dbo
{
    public sealed class MensajesRepository(RealEstateContext realEstateContext,
                                           ILogger<MensajesRepository> logger) : BaseRepository<Mensajes>(realEstateContext), IMensajesRepository
    {
        private readonly RealEstateContext _realEstateContext = realEstateContext;
        private readonly ILogger<MensajesRepository> _logger = logger;

        public async Task<OperationResult> Save(Mensajes mensajes)
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
