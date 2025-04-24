using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Api.Controllers.Base;
using RealEstate.Application.Features.agente.Commands;
using RealEstate.Application.Features.agente.Queries.GetAllAgente;
using RealEstate.Application.Features.agente.Queries.GetAllPropertyByAgent;
using RealEstate.Application.Features.agente.Queries.GetByIDAgente;
using RealEstate.Persistance.Models.dbo;
using RealEstate.Persistance.Models.ViewModel;
using Swashbuckle.AspNetCore.Annotations;

namespace RealEstate.Api.Controllers.v1
{
    [SwaggerTag("Controlador de Agentes")]
    [Authorize(Roles = "Administrador")]
    public class AgentesController : BaseApiController
    {
        [HttpGet("List")]
        [Authorize(Roles = "Administrador,Desarrollador")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AgentesModel))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Listado de Listado",
            Description = "Obtiene un listado de Agentes registrados"
            )]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await Mediator.Send(new GetAllAgenteQuery()));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpGet("GetById/{id}")]
        [Authorize(Roles = "Administrador,Desarrollador")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UsuariosModel))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Agente por Id",
            Description = "Obtiene un Agente por medio de su Id"
            )]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                return Ok(await Mediator.Send(new GetByIDAgenteQuery() { Id = id }));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetAgentProperty{Id}")]
        [Authorize(Roles = "Administrador,Desarrollador")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropiedadesModel))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Propiedades por Agente",
            Description = "Obtiene un listado de las propiedades de un agente por medio del Id del agente"
            )]
        public async Task<IActionResult> GetAgentProperty(string Id)
        {
            try
            {
                return Ok(await Mediator.Send(new GetAllPropertyByAgentQuery() { AgenteID = Id }));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("ChangeStatus/{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Cambiar estado de Agente",
            Description = "Cambia el estado del agente"
            )]
        public async Task<IActionResult> ChangeStatus(string id, [FromBody] ChangeStatusAgenteCommand command)
        {
            command.Id = id;
            try
            {
                return Ok(await Mediator.Send(new ChangeStatusAgenteCommand() { Id = command.Id }));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
