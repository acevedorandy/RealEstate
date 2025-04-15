using AutoMapper;
using Microsoft.Extensions.Logging;
using RealEstate.Application.Contracts.dbo;
using RealEstate.Application.Core;
using RealEstate.Application.Dtos.dbo;
using RealEstate.Domain.Entities.dbo;
using RealEstate.Persistance.Interfaces.dbo;

namespace RealEstate.Application.Services.dbo
{
    public class OfertasService : IOfertasService
    {
        private readonly IOfertasRepository _ofertasRepository;
        private readonly ILogger<OfertasService> _logger;
        private readonly IMapper _mapper;

        public OfertasService(IOfertasRepository ofertasRepository,
                              ILogger<OfertasService> logger,
                              IMapper mapper)
        {
            _ofertasRepository = ofertasRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var result = await _ofertasRepository.GetAll();

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
                response.Messages = "Ha ocurrido un error obteniendo las ofertas.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> GetByIDAsync(int id)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var result = await _ofertasRepository.GetById(id);

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
                response.Messages = "Ha ocurrido un error obteniendo la oferta.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> RemoveAsync(OfertasDto dto)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                Ofertas ofertas = new Ofertas();

                ofertas.OfertaID = dto.OfertaID;
                var result = await _ofertasRepository.Remove(ofertas);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error eliminando la oferta.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> SaveAsync(OfertasDto dto)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var oferta = _mapper.Map<Ofertas>(dto);
                var result = await _ofertasRepository.Save(oferta);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error guardando la oferta.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> UpdateAsync(OfertasDto dto)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var resultGetBy = await _ofertasRepository.GetById(dto.OfertaID);

                if (!resultGetBy.Success)
                {
                    resultGetBy.Success = response.IsSuccess;
                    resultGetBy.Message = response.Messages;

                    return response;
                }

                var oferta = _mapper.Map<Ofertas>(dto);
                var result = await _ofertasRepository.Update(oferta);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error actualizando la oferta.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }
    }
}
