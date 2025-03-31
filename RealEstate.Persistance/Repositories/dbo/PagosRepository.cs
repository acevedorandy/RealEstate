

using Microsoft.Extensions.Logging;
using RealEstate.Domain.Entities.dbo;
using RealEstate.Domain.Result;
using RealEstate.Persistance.Base;
using RealEstate.Persistance.Context;
using RealEstate.Persistance.Interfaces.dbo;

namespace RealEstate.Persistance.Repositories.dbo
{
    public sealed class PagosRepository(RealEstateContext realEstateContext,
                                        ILogger<PagosRepository> logger) : BaseRepository<Pagos>(realEstateContext), IPagosRepository
    {
        private readonly RealEstateContext _realEstateContext = realEstateContext;
        private readonly ILogger<PagosRepository> _logger = logger;

        public async Task<OperationResult> Save(Pagos pagos)
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

        public async Task<OperationResult> Update(Pagos pagos)
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

        public async Task<OperationResult> Remove(Pagos pagos)
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
