using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Contracts.dbo;
using RealEstate.Application.Dtos.dbo;
using RealEstate.Persistance.Models.dbo;

namespace RealEstate.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class PagosController : ControllerBase
    {
        private readonly IPagosService _pagosService;

        public PagosController(IPagosService pagosService)
        {
            _pagosService = pagosService;
        }

        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagosModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get()
        {
            var result = await _pagosService.GetAllAsync();

            if (!result.IsSuccess)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("GetBy{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagosModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _pagosService.GetByIDAsync(id);

            if (!result.IsSuccess)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost("Save")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Post([FromBody] PagosDto dto)
        {
            var result = await _pagosService.SaveAsync(dto);

            if (!result.IsSuccess)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpPut("Update/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagosDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(int id, [FromBody] PagosDto dto)
        {
            dto.ContratoID = id;
            var result = await _pagosService.UpdateAsync(dto);

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
            var dto = new PagosDto
            {
                PagoID = id
            };
            var result = await _pagosService.RemoveAsync(dto);

            if (!result.IsSuccess)
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}
