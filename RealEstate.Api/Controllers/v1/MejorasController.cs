using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Api.Controllers.Base;
using RealEstate.Application.Features.mejora.Commands.RemoveMejoras;
using RealEstate.Application.Features.mejora.Commands.SaveMejoras;
using RealEstate.Application.Features.mejora.Commands.UpdateMejoras;
using RealEstate.Application.Features.mejora.Queries.GetAllMejoras;
using RealEstate.Application.Features.mejora.Queries.GetByIDMejoras;
using RealEstate.Persistance.Models.dbo;
using Swashbuckle.AspNetCore.Annotations;

namespace RealEstate.Api.Controllers.v1
{
    [SwaggerTag("Mantenimiento de Mejoras")]
    [Authorize(Roles = "Administrador")]
    public class MejorasController : BaseApiController
    {
        [HttpPost("Create")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Crear Mejora",
            Description = "Crea/Registra una mejora disponible para las propiedades"
            )]
        public async Task<IActionResult> Post([FromBody]SaveMejorasCommand command)
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
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Actualizar mejora",
            Description = "Actualiza/Modifica una mejora filtrada por medio del Id"
            )]
        public async Task<IActionResult> Put(int id, [FromBody]UpdateMejorasCommand command)
        {
            command.MejoraID = id;
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                if (id != command.MejoraID)
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MejorasModel))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Listado de mejoras",
            Description = "Obtiene un listado de todas las mejoras registradas"
            )]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await Mediator.Send(new GetAllMejorasQuery()));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpGet("GetBy{id}")]
        [Authorize(Roles = "Administrador,Desarrollador")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MejorasModel))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Mejora por Id",
            Description = "Obtiene una mejora por medio de su Id"
            )]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                return Ok(await Mediator.Send(new GetByIDMejorasQuery() { MejoraID = id }));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("Delete{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Eliminar mejora",
            Description = "Elimina una mejora (Este metodo es irreversible)"
            )]
        public async Task<IActionResult> Delete(int id, RemoveMejorasCommand command)
        {
            try
            {
                await Mediator.Send(new RemoveMejorasCommand() { MejoraID = id });
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
