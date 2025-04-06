using AutoMapper;
using Microsoft.Extensions.Logging;
using RealEstate.Application.Contracts.dbo;
using RealEstate.Application.Core;
using RealEstate.Application.Dtos.dbo;
using RealEstate.Domain.Entities.dbo;
using RealEstate.Persistance.Interfaces.dbo;

namespace RealEstate.Application.Services.dbo
{
    public class FavoritosService : IFavoritosService
    {
        private readonly IFavoritosRepository _favoritosRepository;
        private readonly ILogger<FavoritosService> _logger;
        private readonly IMapper _mapper;

        public FavoritosService(IFavoritosRepository favoritosRepository,
                                ILogger<FavoritosService> logger,
                                IMapper mapper)
        {
            _favoritosRepository = favoritosRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var result = await _favoritosRepository.GetAll();

                if (!result.Success)
                {
                    result.Success = response.IsSuccess;
                    result.Message = response.Messages;

                    return response;
                }
                response.Model = result.Data;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error obteniendo las propiedades favoritas.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> GetByIDAsync(int id)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var result = await _favoritosRepository.GetById(id);

                if (!result.Success)
                {
                    result.Success = response.IsSuccess;
                    result.Message = response.Messages;

                    return response;
                }
                response.Model = result.Data;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error obteniendo la propiedad favorita.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> RemoveAsync(FavoritosDto dto)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                Favoritos favoritos = new Favoritos();
                favoritos.FavoritoID = dto.FavoritoID;

                var result = await _favoritosRepository.Remove(favoritos);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error elimninando el favorito.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> SaveAsync(FavoritosDto dto)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var favorito = _mapper.Map<Favoritos>(dto);
                var result = await _favoritosRepository.Save(favorito);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error guardando el favorito.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> UpdateAsync(FavoritosDto dto)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var resultGetBy = await _favoritosRepository.GetById(dto.FavoritoID);

                if (!resultGetBy.Success)
                {
                    response.IsSuccess = resultGetBy.Success;
                    response.Messages = resultGetBy.Message;

                    return response;
                }

                var favorito = _mapper.Map<Favoritos>(dto);
                var result = await _favoritosRepository.Update(favorito);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error actualizando el favorito.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }
    }
}
