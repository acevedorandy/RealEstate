using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Contracts.dbo;
using RealEstate.Application.Dtos.dbo;
using RealEstate.Persistance.Models.dbo;

namespace RealEstate.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class MensajesController : ControllerBase
    {
        private readonly IMensajesService _mensajesService;

        public MensajesController(IMensajesService mensajesService)
        {
            _mensajesService = mensajesService;
        }

        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MensajesModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get()
        {
            var result = await _mensajesService.GetAllAsync();

            if (!result.IsSuccess)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("GetBy{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MensajesModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _mensajesService.GetByIDAsync(id);

            if (!result.IsSuccess)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost("Save")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Post([FromBody] MensajesDto dto)
        {
            var result = await _mensajesService.SaveAsync(dto);

            if (!result.IsSuccess)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpPut("Update/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MensajesDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(int id, [FromBody] MensajesDto dto)
        {
            dto.MensajeID = id;
            var result = await _mensajesService.UpdateAsync(dto);

            if (!result.IsSuccess)
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
            var dto = new MensajesDto
            {
                MensajeID = id
            };
            var result = await _mensajesService.RemoveAsync(dto);

            if (!result.IsSuccess)
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}
