using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Contracts.identity;
using RealEstate.Application.Dtos.identity;
using Swashbuckle.AspNetCore.Annotations;

namespace RealEstate.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Logeo y Registro")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountServiceForWebApi _accountService;

        public AccountController(IAccountServiceForWebApi accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("authenticate")]
        [SwaggerOperation(
            Summary = "Autenticar/Logeo",
            Description = "Obten un Token al logearse .Debe tener una cuenta registrada(Administrador o Desarrollador)"
            )]
        public async Task<IActionResult> AuthenticateAsync(AuthenticationRequest request)
        {
            return Ok(await _accountService.AuthenticateAsync(request));
        }

        [HttpPost("registerDesarrollador")]
        [SwaggerOperation(
            Summary = "Registro de Desarrollador",
            Description = "Crea/registra un usuario con el rol de Desarrollador"
            )]
        public async Task<IActionResult> RegisterDesarrolladorAsync(RegisterRequest request)
        {
            var origin = Request.Headers["origin"];
            request.Rol = "Desarrollador";
            return Ok(await _accountService.RegisterIdentityAsync(request, origin));
        }

        [HttpPost("registerAdministrador")]
        [Authorize(Roles = "Administrador")]
        [SwaggerOperation(
            Summary = "Registro de Administrador",
            Description = "Crea/registra un usuario con el rol de Adminstrador. Solo un usuario Logeado con el role de Administrador puede usar este EndPoint"
            )]
        public async Task<IActionResult> RegisterAdministradorAsync([FromBody] RegisterRequest request)
        {
            var origin = Request.Headers["origin"];
            request.Rol = "Administrador";
            return Ok(await _accountService.RegisterIdentityAsync(request, origin));
        }
    }
}
