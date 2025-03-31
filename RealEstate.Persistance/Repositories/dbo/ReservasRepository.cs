

using Microsoft.Extensions.Logging;
using RealEstate.Domain.Entities.dbo;
using RealEstate.Domain.Result;
using RealEstate.Persistance.Base;
using RealEstate.Persistance.Context;
using RealEstate.Persistance.Interfaces.dbo;

namespace RealEstate.Persistance.Repositories.dbo
{
    public sealed class ReservasRepository(RealEstateContext realEstateContext,
                                          ILogger<ReservasRepository> logger) : BaseRepository<Reservas>(realEstateContext), IReservasRepository
    {
        private readonly RealEstateContext _realEstateContext = realEstateContext;
        private readonly ILogger<ReservasRepository> _logger = logger;

        public async Task<OperationResult> Save(Reservas reservas)
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

        public async Task<OperationResult> Update(Reservas reservas)
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

        public async Task<OperationResult> Remove(Reservas reservas)
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
