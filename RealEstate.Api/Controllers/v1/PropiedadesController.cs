using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Contracts.dbo;
using RealEstate.Persistance.Models.dbo;

namespace RealEstate.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version}/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrador,Desarrollador")]
    public class PropiedadesController : ControllerBase
    {
        private readonly IPropiedadesService _propiedadesService;

        public PropiedadesController(IPropiedadesService propiedadesService)
        {
            _propiedadesService = propiedadesService;
        }

        [HttpGet("List")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropiedadesModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var propiedades = await _propiedadesService.GetAllAsync();

                if (!propiedades.IsSuccess)
                {
                    return NotFound();
                }

                return Ok(propiedades);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            
        }

        [HttpGet("GetBy{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropiedadesModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var propiedad = await _propiedadesService.GetByIDAsync(id);

                if (!propiedad.IsSuccess)
                {
                    return NotFound();
                }

                return Ok(propiedad);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /*[HttpGet("GetAllPropertyByAgentAsync")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropiedadesModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllPropertyByAgentAsync()
        {
            var propiedades = await _propiedadesService.GetAllPropertyByAgentAsync();

            if (!propiedades.IsSuccess)
            {
                return NotFound();
            }

            return Ok(propiedades);
        }

        [HttpGet("GetAllFilter/{tipoPropiedad}/{minPrice}/{maxPrice}/{habitacion}/{baños}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropiedadesModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllFilter(string tipoPropiedad, decimal? minPrice, decimal? maxPrice, int? habitacion, int? baños)
        {
            try
            {
                var propiedades = await _propiedadesService.GetAllFilter(tipoPropiedad, minPrice, maxPrice, habitacion, baños);

                if (!propiedades.IsSuccess)
                {
                    return NotFound();
                }

                return Ok(propiedades);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetAgentByPropertyAsync/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UsuariosModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAgentByPropertyAsync(int propiedadId)
        {
            try
            {
                var agente = await _propiedadesService.GetAgentByPropertyAsync(propiedadId);

                if (!agente.IsSuccess)
                {
                    return NotFound();
                }

                return Ok(agente);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("Save")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] PropiedadesDto dto)
        {
            try
            {
                var propiedades = await _propiedadesService.SaveAsync(dto);

                if (!propiedades.IsSuccess)
                {
                    return BadRequest();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("Update/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropiedadesDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(int id, [FromBody] PropiedadesDto dto)
        {
            try
            {
                dto.PropiedadID = id;
                var propiedades = await _propiedadesService.UpdateAsync(dto);

                if (!propiedades.IsSuccess)
                {
                    return BadRequest();
                }

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("Delete/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var dto = new PropiedadesDto
                {
                    PropiedadID = id
                };
                var propiedades = await _propiedadesService.RemoveAsync(dto);

                if (!propiedades.IsSuccess)
                {
                    return BadRequest();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }*/
    }
}
