using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Contracts.dbo;
using RealEstate.Application.Dtos.identity.account;
using RealEstate.Application.Responses.identity;
using RealEstate.Identity.Shared.Entities;
using RealEstate.Application.Helpers.web;
using RealEstate.Web.Helpers.Otros;


namespace RealEstate.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUsuariosService _usuariosService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SelectListHelper _selectRol;

        public AccountController(IUsuariosService usuariosService,
                                 UserManager<ApplicationUser> userManager,
                                 SelectListHelper selectRol)
        {
            _usuariosService = usuariosService;
            _userManager = userManager;
            _selectRol = selectRol;
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

            if (authentication != null && authentication.HasError != true)
            {
                // Guardar el usuario en la sesión
                HttpContext.Session.Set<AuthenticationResponse>("usuario", authentication);

                // Obtener el usuario autenticado
                //var user = await _userManager.FindByEmailAsync(loginDto.Email);

                //if (await _userManager.IsInRoleAsync(user, "Admin"))
                //{
                //    return RedirectToAction("Welcome", "Account");
                //}
                //else if (await _userManager.IsInRoleAsync(user, "Basic"))
                //{
                //    return RedirectToAction("Welcome", "Account"); 
                //}
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
    }
}
