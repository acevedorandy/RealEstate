using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RealEstate.Application.Contracts.dbo;
using RealEstate.Application.Core;
using RealEstate.Application.Dtos.dbo;
using RealEstate.Application.Responses.identity;
using RealEstate.Domain.Entities.dbo;
using RealEstate.Persistance.Interfaces.dbo;
using RealEstate.Application.Helpers.web;
using RealEstate.Application.Helpers;
using RealEstate.Persistance.Models.dbo;


namespace RealEstate.Application.Services.dbo
{
    public class PropiedadesService : IPropiedadesService
    {
        private readonly IPropiedadesRepository _propiedadesRepository;
        private readonly ILogger<PropiedadesService> _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse authenticationResponse;
        private readonly IPropiedadFotosRepository _propiedadFotosRepository;
        private readonly IUsuariosRepository _usuariosRepository;

        public PropiedadesService(IPropiedadesRepository propiedadesRepository,
                                  ILogger<PropiedadesService> logger,
                                  IMapper mapper,
                                  IHttpContextAccessor httpContextAccessor,
                                  IPropiedadFotosRepository propiedadFotosRepository,
                                  IUsuariosRepository usuariosRepository)
        {
            _propiedadesRepository = propiedadesRepository;
            _logger = logger;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            authenticationResponse = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("usuario");
            _propiedadFotosRepository = propiedadFotosRepository;
            _usuariosRepository = usuariosRepository;
        }

        public async Task<ServiceResponse> GetAgentByPropertyAsync(int propiedadId)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var result = await _propiedadesRepository.GetAgentByProperty(propiedadId);

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
                response.Messages = "Ha ocurrido un error obteniendo la propiedad.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var result = await _propiedadesRepository.GetAll();

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
                response.Messages = "Ha ocurrido un error obteniendo las propiedades.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> GetAllFilter(string tipoPropiedad, decimal? minPrice, decimal? maxPrice, int? habitacion, int? baños)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var result = await _propiedadesRepository.GetAll();

                if (!result.Success)
                {
                    response.IsSuccess = result.Success;
                    response.Messages = result.Message;
                    return response;
                }

                var propiedades = ((List<PropiedadesModel>)result.Data).AsQueryable();

                if (!string.IsNullOrEmpty(tipoPropiedad))
                {
                    if (int.TryParse(tipoPropiedad, out int tipoPropiedadId))
                    {
                        propiedades = propiedades.Where(p => p.TipoPropiedad == tipoPropiedadId);
                    }
                }

                if (minPrice.HasValue)
                    propiedades = propiedades.Where(p => p.Precio >= minPrice.Value);

                if (maxPrice.HasValue)
                    propiedades = propiedades.Where(p => p.Precio <= maxPrice.Value);

                if (habitacion.HasValue)
                    propiedades = propiedades.Where(p => p.Habitaciones == habitacion.Value);

                if (baños.HasValue)
                    propiedades = propiedades.Where(p => p.Baños == baños.Value);

                response.Model = propiedades.ToList();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error obteniendo las propiedades filtradas.";
                _logger.LogError(response.Messages, ex.ToString());
            }

            return response;
        }

        public async Task<ServiceResponse> GetAllPropertyByAgentAsync(string id)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var result = await _propiedadesRepository.GetAllPropertyByAgent(id);

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
                response.Messages = "Ha ocurrido un error obteniendo las propiedades.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> GetAllPropertyByAgentLogged()
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                string id = authenticationResponse.Id;
                var result = await _propiedadesRepository.GetAllPropertyByAgent(id);

                if (!result.Success)
                {
                    response.IsSuccess = result.Success;
                    response.Messages = result.Message;
                    return response;
                }

                var propiedades = ((IEnumerable<dynamic>)result.Data)
                    .Select(p => new PropiedadesModel
                    {
                        PropiedadID = p.Id,
                        Titulo = p.Titulo,
                        Descripcion = p.Descripcion,
                        Disponibilidad = p.Disponibilidad,
                        Vendida = p.Vendida,
                        Precio = p.Precio,
                    })
                    .Where(p => p.Disponibilidad == true && p.Vendida == false) 
                    .ToList();

                response.Model = propiedades;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error obteniendo las propiedades del agente logueado.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> GetAllPropertyByAgentIncludeSold()
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                string id = authenticationResponse.Id;
                var result = await _propiedadesRepository.GetAllPropertyByAgent(id);

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
                response.Messages = "Ha ocurrido un error obteniendo las propiedades del agente logueado.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }


        public async Task<ServiceResponse> GetByIDAsync(int id)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var result = await _propiedadesRepository.GetById(id);

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
                response.Messages = "Ha ocurrido un error obteniendo la propiedad.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> RemoveAsync(PropiedadesDto dto)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                Propiedades propiedades = new Propiedades();

                propiedades.PropiedadID = dto.PropiedadID;
                var result = await _propiedadesRepository.Remove(propiedades);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error eliminando la propiedad.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> SaveAsync(PropiedadesDto dto)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                dto.AgenteID = authenticationResponse.Id;
                dto.Codigo = NumberGenerator.CodeGenerator();

                dto.Vendida = false;
                var propiedad = _mapper.Map<Propiedades>(dto);
                var result = await _propiedadesRepository.Save(propiedad);

                dto.PropiedadID = propiedad.PropiedadID;
                response.Model = dto;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error guardando la propiedad.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> UpdateAsync(PropiedadesDto dto)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var resultGetBy = await _propiedadesRepository.GetById(dto.PropiedadID);

                if (!resultGetBy.Success)
                {
                    resultGetBy.Success = response.IsSuccess;
                    resultGetBy.Message = response.Messages;

                    return response;
                }

                var propiedad = _mapper.Map<Propiedades>(dto);
                var result = await _propiedadesRepository.Update(propiedad);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error actualizando la propiedad.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> GetAllPropertyNotSold()
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var result = await _propiedadesRepository.GetAll();

                if (!result.Success)
                {
                    response.IsSuccess = result.Success;
                    response.Messages = result.Message;
                    return response;
                }

                var propiedades = result.Data as List<PropiedadesModel>;

                response.Model = propiedades
                    .Where(p => p.Disponibilidad == true && p.Vendida == false)
                    .ToList();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error obteniendo las propiedades del agente logueado.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }
    }
}
