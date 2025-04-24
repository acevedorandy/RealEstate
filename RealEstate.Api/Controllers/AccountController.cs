using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Contracts.identity;
using RealEstate.Application.Dtos.identity;

namespace RealEstate.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountServiceForWebApi _accountService;

        public AccountController(IAccountServiceForWebApi accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync(AuthenticationRequest request)
        {
            return Ok(await _accountService.AuthenticateAsync(request));
        }

        [HttpPost("registerDesarrollador")]
        public async Task<IActionResult> RegisterDesarrolladorAsync(RegisterRequest request)
        {
            var origin = Request.Headers["origin"];
            request.Rol = "Desarrollador";
            return Ok(await _accountService.RegisterIdentityAsync(request, origin));
        }

        [HttpPost("registerAdministrador")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> RegisterAdministradorAsync(RegisterRequest request)
        {
            var origin = Request.Headers["origin"];
            request.Rol = "Administrador";
            return Ok(await _accountService.RegisterIdentityAsync(request, origin));
        }
    }
}
