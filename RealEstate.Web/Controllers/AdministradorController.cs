using Azure;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Contracts.dbo;
using RealEstate.Application.Dtos.dbo;
using RealEstate.Application.Dtos.identity.account;
using RealEstate.Application.Responses.identity;
using RealEstate.Application.Services.dbo;
using RealEstate.Persistance.Models.dbo;
using RealEstate.Persistance.Models.ViewModel;

namespace RealEstate.Web.Controllers
{
    public class AdministradorController : Controller
    {
        private readonly IUsuariosService _usuariosService;

        public AdministradorController(IUsuariosService usuariosService)
        {
            _usuariosService = usuariosService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _usuariosService.LoadHomeView();

            if (result.IsSuccess)
            {
                HomeAdmin model = (HomeAdmin)result.Model;
                return View(model);
            }
            return View();
        }

        public async Task<IActionResult> ListaAgentes()
        {
            var result = await _usuariosService.GetAllAgentAsync();

            if (result.IsSuccess)
            {
                List<AgentesModel> agentes = (List<AgentesModel>)result.Model;
                return View(agentes);
            }
            return View();
        }

        public async Task<IActionResult> ListaDesarrolladores()
        {
            var result = await _usuariosService.GetAllDeveloperAsync();

            if (result.IsSuccess)
            {
                List<DesarrolladorModel> agentes = (List<DesarrolladorModel>)result.Model;
                return View(agentes);
            }
            return View();
        }

        public async Task<IActionResult> ListaAdministradores()
        {
            var result = await _usuariosService.GetAllAdminsAsync();

            if (result.IsSuccess)
            {
                List<AdministradorModel> agentes = (List<AdministradorModel>)result.Model;
                return View(agentes);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearDesarrolladores(RegisterDto dto)
        {
            var origen = Request.Headers["origin"];

            RegisterResponse response = await _usuariosService.RegisterAsync(dto, origen);

            if (response.HasError)
            {
                dto.HasError = response.HasError;
                dto.Error = response.Error;
                TempData["ErrorMessage"] = response.Error;
                return RedirectToRoute(new { controller = "Administrador", action = "ListaDesarrolladores" });
            }
            TempData["SuccessMessage"] = "Desarrollador añadido exitosamente.";
            return RedirectToRoute(new { controller = "Administrador", action = "ListaDesarrolladores" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActivarOrDesactivar(string id)
        {
            try
            {
                var result = await _usuariosService.ActivarOrDesactivarAsync(id);

                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Activación/Desactivación exitosa.";
                    return Json(new { success = true });
                }

                TempData["ErrorMessage"] = result.Messages;
                return Json(new { success = false, message = result.Messages });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditarUsuario(string id)
        {
            var result = await _usuariosService.GetIdentityUserByAsync(id);

            if (result.IsSuccess)
            {
                UsuariosModel usuarios = (UsuariosModel)result.Model;
                return View(usuarios);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarUsuario(UsuariosDto dto)
        {
            try
            {
                var result = await _usuariosService.UpdateIdentityUserAsync(dto);

                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Usuario actualizado exitosamente.";
                    return RedirectToRoute(new { controller = "Administrador", action = "ListaDesarrolladores" });
                }
                else
                {
                    TempData["ErrorMessage"] = result.Messages;
                    return RedirectToRoute(new { controller = "Administrador", action = "ListaDesarrolladores" });
                }
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Delete(UsuariosDto dto)
        {
            try
            {
                var result = await _usuariosService.RemoveAgentWithPropertyAsync(dto.Id);

                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "El agente y todas sus propiedades fueron eliminado.";
                    return RedirectToAction("ListaAgentes", "Administrador");
                }
                else
                {
                    TempData["ErrorMessage"] = result.Messages;
                    return RedirectToAction("ListaAgentes", "Administrador");
                }
            }
            catch
            {
                return View();
            }
        }
    }
}
