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

        /*[HttpGet("confirm-email")]
        public async Task<IActionResult> RegisterAsync([FromQuery] string userId, [FromQuery] string token)
        {
            return Ok(await _accountService.ConfirmAccountAsync(userId, token));
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            var origin = Request.Headers["origin"];
            return Ok(await _accountService.ForgotPasswordAsync(request, origin));
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordRequest request)
        {
            return Ok(await _accountService.ResetPasswordAsync(request));
        }*/
    }
}
