using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Api.Controllers.Base;
using RealEstate.Application.Features.tipoPropiedad.Commands.RemoveTiposPropiedad;
using RealEstate.Application.Features.tipoPropiedad.Commands.SaveTiposPropiedad;
using RealEstate.Application.Features.tipoPropiedad.Commands.UpdateTiposPropiedad;
using RealEstate.Application.Features.tipoPropiedad.Queries.GetAllTiposPropiedad;
using RealEstate.Application.Features.tipoPropiedad.Queries.GetByIDTiposPropiedad;
using RealEstate.Persistance.Models.dbo;
using Swashbuckle.AspNetCore.Annotations;

namespace RealEstate.Api.Controllers.v1
{
    [SwaggerTag("Mantenimiento de Tipos de Propiedades")]
    [Authorize(Roles = "Administrador")]
    public class TiposPropiedadController : BaseApiController
    {
        [HttpPost("Create")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Crear Tipo de Propiedad",
            Description = "Crea/Guarda un Tipo de Propiedad"
            )]
        public async Task<IActionResult> Post([FromBody] SaveTiposPropiedadCommand command)
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TiposPropiedadModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Actualizar Tipo de Propiedad",
            Description = "Actializa/Modifica un Tipo de Propiedad por medio del Id"
            )]
        public  async Task<IActionResult> Put(int id, [FromBody] UpdateTiposPropiedadCommand command)
        {
            command.TipoPropiedadID = id;
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                if(id != command.TipoPropiedadID)
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TiposPropiedadModel))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Listado de Tipos de Propiedades",
            Description = "Obtiene un listado de los Tipos de Propiedades registrados"
            )]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await Mediator.Send(new GetAllTiposPropiedadQuery()));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpGet("GetBy{id}")]
        [Authorize(Roles = "Administrador,Desarrollador")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TiposPropiedadModel))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Tipos de Propiedades por Id",
            Description = "Obtiene un Tipo de Propiedad por medio del Id"
            )]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                return Ok(await Mediator.Send(new GetByIDTiposPropiedadQuery() { TipoPropiedadID = id }));
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
            Summary = "Eliminar Tipo de porpiedad",
            Description = "Elimina un Tipo de Propiedad (Este metodo es irreversible)"
            )]
        public async Task<IActionResult> Delete(int id, RemoveTiposPropiedadCommand command)
        {
            try
            {
                await Mediator.Send(new RemoveTiposPropiedadCommand() { TipoPropiedadID = id });
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
