

using Microsoft.Extensions.Logging;
using RealEstate.Domain.Entities.dbo;
using RealEstate.Domain.Result;
using RealEstate.Persistance.Base;
using RealEstate.Persistance.Context;
using RealEstate.Persistance.Interfaces.dbo;

namespace RealEstate.Persistance.Repositories.dbo
{
    public sealed class FavoritosRepository(RealEstateContext realEstateContext,
                                            ILogger<FavoritosRepository> logger) : BaseRepository<Favoritos>(realEstateContext), IFavoritosRepository
    {
        private readonly RealEstateContext _realEstateContext = realEstateContext;
        private readonly ILogger<FavoritosRepository> _logger = logger;

        public async Task<OperationResult> Save(Favoritos favoritos)
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

        public async Task<OperationResult> Update(Favoritos favoritos)
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

        public async Task<OperationResult> Remove(Favoritos favoritos)
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
