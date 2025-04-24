using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Api.Controllers.Base;
using RealEstate.Application.Features.tipoVenta.Commands.RemoveTiposVenta;
using RealEstate.Application.Features.tipoVenta.Commands.SaveTiposVenta;
using RealEstate.Application.Features.tipoVenta.Commands.UpdateTiposVenta;
using RealEstate.Application.Features.tipoVenta.Queries.GetAllPropiedades;
using RealEstate.Application.Features.tipoVenta.Queries.GetByIDPropiedades;
using RealEstate.Persistance.Models.dbo;

namespace RealEstate.Api.Controllers.v1
{
    /*[Route("api/[controller]")]
    [ApiController]*/
    [Authorize(Roles = "Administrador")]
    public class TiposVentaController : BaseApiController
    {
        [HttpPost("Create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(SaveTiposVentaCommand command)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                await Mediator.Send(command);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("Update/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TiposVentaModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(int id, UpdateTiposVentaCommand command)
        {
            command.TipoVentaID = id;
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                if (id != command.TipoVentaID)
                {
                    return BadRequest();
                }
                await Mediator.Send(command);

                return Ok(command);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("List")]
        [Authorize(Roles = "Administrador,Desarrollador")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TiposVentaModel))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await Mediator.Send(new GetAllTiposVentaQuery()));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpGet("GetBy{id}")]
        [Authorize(Roles = "Administrador,Desarrollador")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TiposVentaModel))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                return Ok(await Mediator.Send(new GetByIDTiposVentaQuery() { TipoVentaID = id }));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("Delete{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id, RemoveTiposVentaCommand command)
        {
            try
            {
                await Mediator.Send(new RemoveTiposVentaCommand() { TipoVentaID = id });
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
