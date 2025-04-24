using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Api.Controllers.Base;
using RealEstate.Application.Features.propiedad.Queries.GetAllPropiedades;
using RealEstate.Application.Features.propiedad.Queries.GetByCodePropiedades;
using RealEstate.Application.Features.propiedad.Queries.GetByIDPropiedades;
using RealEstate.Persistance.Models.dbo;
using Swashbuckle.AspNetCore.Annotations;

namespace RealEstate.Api.Controllers.v1
{
    [SwaggerTag("Controlador de Propiedades")]
    [Authorize(Roles = "Administrador,Desarrollador")]
    public class PropiedadesController : BaseApiController
    {
        [HttpGet("List")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropiedadesModel))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Listado de propiedades",
            Description = "Obtiene un listado de todas las propiedades registradas"
            )]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await Mediator.Send(new GetAllPropiedadesQuery()));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            
        }

        [HttpGet("GetById/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropiedadesModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Propiedad por Id",
            Description = "Obtiene una propiedad por medio de su Id"
            )]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                return Ok(await Mediator.Send(new GetByIDPropiedadesQuery() { PropiedadID = id }));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetBy{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropiedadesModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Propiedad por codigo",
            Description = "Obtiene una propiedad por medio de su codigo"
            )]
        public async Task<IActionResult> GetByCode(string code)
        {
            string Code = code;
            try
            {
                return Ok(await Mediator.Send(new GetByCodePropiedadesQuery() { Codigo = code }));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
