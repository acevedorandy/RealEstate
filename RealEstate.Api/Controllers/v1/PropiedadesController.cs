using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Contracts.dbo;
using RealEstate.Application.Dtos.dbo;
using RealEstate.Persistance.Models.dbo;

namespace RealEstate.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class PropiedadesController : ControllerBase
    {
        private readonly IPropiedadesService _propiedadesService;

        public PropiedadesController(IPropiedadesService propiedadesService)
        {
            _propiedadesService = propiedadesService;
        }

        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropiedadesModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get()
        {
            var propiedades = await _propiedadesService.GetAllAsync();

            if (!propiedades.IsSuccess)
            {
                return NotFound();
            }

            return Ok(propiedades);
        }

        [HttpGet("GetBy{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropiedadesModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var propiedad = await _propiedadesService.GetByIDAsync(id);

            if (!propiedad.IsSuccess)
            {
                return NotFound();
            }

            return Ok(propiedad);
        }

        [HttpPost("Save")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Post([FromBody] PropiedadesDto dto)
        {
            var propiedades = await _propiedadesService.SaveAsync(dto);

            if (!propiedades.IsSuccess)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpPut("Update/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropiedadesDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(int id, [FromBody] PropiedadesDto dto)
        {
            dto.PropiedadID = id;
            var propiedades = await _propiedadesService.UpdateAsync(dto);

            if (!propiedades.IsSuccess)
            {
                return BadRequest();
            }

            return Ok(dto);
        }

        [HttpDelete("Delete/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
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
    }
}
