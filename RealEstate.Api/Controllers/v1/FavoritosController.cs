using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Contracts.dbo;
using RealEstate.Application.Dtos.dbo;
using RealEstate.Persistance.Models.dbo;

namespace RealEstate.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class FavoritosController : ControllerBase
    {
        private readonly IFavoritosService _favoriteService;

        public FavoritosController(IFavoritosService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FavoritosModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get()
        {
            var result = await _favoriteService.GetAllAsync();

            if (!result.IsSuccess)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("GetBy{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FavoritosModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _favoriteService.GetByIDAsync(id);

            if (!result.IsSuccess)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost("Save")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Post([FromBody] FavoritosDto dto)
        {
            var result = await _favoriteService.SaveAsync(dto);

            if (!result.IsSuccess)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpPut("Update/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FavoritosDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(int id, [FromBody] FavoritosDto dto)
        {
            dto.FavoritoID = id;
            var result = await _favoriteService.UpdateAsync(dto);

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
            var dto = new FavoritosDto
            {
                FavoritoID = id
            };
            var result = await _favoriteService.RemoveAsync(dto);

            if (!result.IsSuccess)
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}
