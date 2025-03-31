

using Microsoft.Extensions.Logging;
using RealEstate.Domain.Entities.dbo;
using RealEstate.Domain.Result;
using RealEstate.Persistance.Base;
using RealEstate.Persistance.Context;
using RealEstate.Persistance.Interfaces.dbo;

namespace RealEstate.Persistance.Repositories.dbo
{
    public sealed class ContratorRepository(RealEstateContext realEstateContext,
                                            ILogger<ContratorRepository> logger) : BaseRepository<Contratos>(realEstateContext), IContratosRepository
    {
        private readonly RealEstateContext _realEstateContext = realEstateContext;
        private readonly ILogger<ContratorRepository> _logger = logger;

        public async Task<OperationResult> Save(Contratos contratos)
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

        public async Task<OperationResult> Update(Contratos contratos)
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

        public async Task<OperationResult> Remove(Contratos contratos)
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
