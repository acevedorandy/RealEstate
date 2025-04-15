using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Contracts.dbo;
using RealEstate.Persistance.Models.dbo;

namespace RealEstate.Web.Controllers
{
    public class AgentesController : Controller
    {
        private readonly IUsuariosService _usuariosService;

        public AgentesController(IUsuariosService usuariosService)
        {
            _usuariosService = usuariosService;
        }

        public async Task <IActionResult> Index()
        {
            var result = await _usuariosService.GetAgentActiveAsync();

            if (result.IsSuccess)
            {
                List<UsuariosModel> usuarios = (List<UsuariosModel>)result.Model;
                return View(usuarios);
            }
            return View();
        }

        public async Task<IActionResult> Buscar(string nombre)
        {
            var result = await _usuariosService.GetAgentByNameAsync(nombre);

            if (result.IsSuccess)
            {
                UsuariosModel usuarios = (UsuariosModel)result.Model;
                return View(usuarios);
            }

            return View();
        }

        public ActionResult Details(int id)
        {
            return View();
        }

    }
}
