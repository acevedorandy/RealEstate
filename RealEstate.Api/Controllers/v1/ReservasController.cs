using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Contracts.dbo;
using RealEstate.Application.Dtos.dbo;
using RealEstate.Persistance.Models.dbo;

namespace RealEstate.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ReservasController : ControllerBase
    {
        private readonly IReservasService _reservasService;

        public ReservasController(IReservasService reservasService)
        {
            _reservasService = reservasService;
        }

        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReservasModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get()
        {
            var result = await _reservasService.GetAllAsync();

            if (!result.IsSuccess)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("GetBy{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReservasModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _reservasService.GetByIDAsync(id);

            if (!result.IsSuccess)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost("Save")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Post([FromBody] ReservasDto dto)
        {
            var result = await _reservasService.SaveAsync(dto);

            if (!result.IsSuccess)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpPut("Update/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReservasDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(int id, [FromBody] ReservasDto dto)
        {
            dto.ReservaID = id;
            var result = await _reservasService.UpdateAsync(dto);

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
            var dto = new ReservasDto
            {
                ReservaID = id
            };
            var result = await _reservasService.RemoveAsync(dto);

            if (!result.IsSuccess)
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}
