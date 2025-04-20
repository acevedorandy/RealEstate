using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Contracts.dbo;
using RealEstate.Application.Dtos.dbo;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActivarOrDesactivar(string id)
        {
            try
            {
                var result = await _usuariosService.ActivarOrDesactivarAsync(id);

                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }

                return Json(new { success = false, message = result.Messages });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
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
