using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Contracts.dbo;
using RealEstate.Application.Dtos.identity.account;
using RealEstate.Application.Responses.identity;
using RealEstate.Identity.Shared.Entities;
using RealEstate.Application.Helpers.web;
using RealEstate.Web.Helpers.Otros;
using RealEstate.Application.Dtos.dbo;
using RealEstate.Persistance.Models.dbo;
using System.Security.Claims;
using RealEstate.Web.Helpers.Imagenes;
using RealEstate.Persistance.Models.ViewModel;
using RealEstate.Application.Models;


namespace RealEstate.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUsuariosService _usuariosService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SelectListHelper _selectRol;
        private readonly ImagenHelper _imagenHelper;

        public AccountController(IUsuariosService usuariosService,
                                 UserManager<ApplicationUser> userManager,
                                 SelectListHelper selectRol,
                                 ImagenHelper imagenHelper)
        {
            _usuariosService = usuariosService;
            _userManager = userManager;
            _selectRol = selectRol;
            _imagenHelper = imagenHelper;
        }

        public IActionResult Index()
        {
            return View(new LoginDto());
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return View(loginDto);
            }

            AuthenticationResponse authentication = await _usuariosService.LoginAsync(loginDto);

            if (authentication != null && !authentication.HasError)
            {
                HttpContext.Session.Set("usuario", authentication);

                // Verifica los roles
                if (authentication.Roles.Contains("Administrador"))
                {
                    return RedirectToAction("Index", "Administrador");
                }
                else if (authentication.Roles.Contains("Agente"))
                {
                    return RedirectToAction("Home", "Agentes");
                }
                else if (authentication.Roles.Contains("Cliente"))
                {
                    return RedirectToAction("Index", "Clientes");
                }

                // Por defecto si no tiene ninguno de los roles
                return RedirectToAction("Welcome", "Account");
            }
            else
            {
                loginDto.HasError = authentication.HasError;
                loginDto.Error = authentication.Error;
                return View(loginDto);
            }
        }




        public IActionResult Register()
        {
            var roles = _selectRol.Roles();
            ViewBag.Rol = roles;

            return View(new RegisterDto());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (ModelState.IsValid)
            {
                return View(registerDto);
            }
            var origen = Request.Headers["origin"];

            RegisterResponse response = await _usuariosService.RegisterAsync(registerDto, origen);

            if (response.HasError)
            {
                registerDto.HasError = response.HasError;
                registerDto.Error = response.Error;
                return View(registerDto);
            }
            return RedirectToRoute(new { controller = "Account", action = "Welcome" });
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            string response = await _usuariosService.ConfirmEmailAsync(userId, token);
            return View("ConfirmEmail", response);
        }

        public async Task<IActionResult> LogOut()
        {
            await _usuariosService.SignOutAsync();
            HttpContext.Session.Remove("usuario");

            return RedirectToRoute(new { controller = "Account", action = "Index" });
        }

        public IActionResult ForgotPassword()
        {
            return View(new ForgotPasswordDto());
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return View(forgotPasswordDto);
            }

            var origin = Request.Headers["origin"];

            ForgotPasswordResponse response = await _usuariosService.ForgotPasswordAsync(forgotPasswordDto, origin);

            if (response.HasError)
            {
                forgotPasswordDto.HasError = response.HasError;
                forgotPasswordDto.Error = response.Error;
                return View(forgotPasswordDto);
            }

            return RedirectToRoute(new { controller = "Account", action = "Index" });
        }

        public IActionResult ResetPassword(string token)
        {
            return View(new ResetPasswordDto { Token = token });
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return View(resetPasswordDto);
            }

            ResetPasswordResponse response = await _usuariosService.ResetPasswordAsync(resetPasswordDto);

            if (response.HasError)
            {
                resetPasswordDto.HasError = response.HasError;
                resetPasswordDto.Error = response.Error;
                return View(resetPasswordDto);
            }
            return RedirectToRoute(new { controller = "Account", action = "Index" });
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
        public IActionResult Welcome()
        {
            return View();
        }

        public async Task<IActionResult> MiPerfil()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _usuariosService.GetIdentityUserByAsync(userId);

            if (result.IsSuccess)
            {
                UsuariosModel usuarios = (UsuariosModel)result.Model;
                return View(usuarios);
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditarPerfil(string id)
        {
            var result = await _usuariosService.GetPerfilInformation(id);

            if (result.IsSuccess)
            {
                PerfilModel usuarios = (PerfilModel)result.Model;
                return View(usuarios);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarPerfil(UsuariosDto dto, IFormFile foto)
        {
            try
            {
                var result = await _usuariosService.UpdateIdentityUserAsync(dto);

                if (result.IsSuccess)
                {
                    dto = result.Model;
                    dto = await _imagenHelper.UpdatePerfilPhoto(dto, foto);
                    var updateResult = await _usuariosService.UpdateIdentityUserAsync(dto);

                    if (!updateResult.IsSuccess)
                    {
                        TempData["ErrorMessage"] = result.Messages;
                        return RedirectToRoute(new { controller = "Account", action = "MiPerfil" });
                    }

                    TempData["SuccessMessage"] = "Informacion actualizada exitosamente.";
                    return RedirectToRoute(new { controller = "Account", action = "MiPerfil" });
                    //TempData["SuccessMessage"] = "Informacion actualizada exitosamente.";
                    //return RedirectToRoute(new { controller = "Account", action = "MiPerfil" });
                }
                else
                {
                    TempData["ErrorMessage"] = result.Messages;
                    return RedirectToRoute(new { controller = "Account", action = "MiPerfil" });
                }
            }
            catch
            {
                return View();
            }
        }
    }
}
