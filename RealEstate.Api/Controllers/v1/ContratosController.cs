using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Contracts.dbo;
using RealEstate.Application.Dtos.dbo;
using RealEstate.Persistance.Models.dbo;

namespace RealEstate.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ContratosController : ControllerBase
    {
        private readonly IContratosService _contratosService;

        public ContratosController(IContratosService contratosService)
        {
            _contratosService = contratosService;
        }

        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ContratosModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get()
        {
            var result = await _contratosService.GetAllAsync();

            if (!result.IsSuccess)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("GetBy{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ContratosModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _contratosService.GetByIDAsync(id);

            if (!result.IsSuccess)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost("Save")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Post([FromBody] ContratosDto dto)
        {
            var result = await _contratosService.SaveAsync(dto);

            if (!result.IsSuccess)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpPut("Update/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ContratosDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(int id, [FromBody] ContratosDto dto)
        {
            dto.ContratoID = id;
            var result = await _contratosService.UpdateAsync(dto);

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
            var dto = new ContratosDto
            {
                ContratoID = id
            };
            var result = await _contratosService.RemoveAsync(dto);

            if (!result.IsSuccess)
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}
