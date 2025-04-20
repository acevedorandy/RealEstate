using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Contracts.dbo;
using RealEstate.Persistance.Models.dbo;

namespace RealEstate.Web.Controllers
{
    public class AgentesController : Controller
    {
        private readonly IUsuariosService _usuariosService;
        private readonly IPropiedadesService _propiedadesService;

        public AgentesController(IUsuariosService usuariosService,
                                 IPropiedadesService propiedadesService)
        {
            _usuariosService = usuariosService;
            _propiedadesService = propiedadesService;
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

        public async Task<IActionResult> PropiedadesAgente(string agenteId)
        {
            var result = await _propiedadesService.GetAllPropertyByAgentAsync(agenteId);

            if (result.IsSuccess)
            {
                List<PropiedadesModel> propiedades = (List<PropiedadesModel>)result.Model;
                return View(propiedades);
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

        public async Task<IActionResult> Home()
        {
            var result = await _propiedadesService.GetAllPropertyByAgentIncludeSold();

            if (result.IsSuccess)
            {
                List<PropiedadesModel> propiedades = (List<PropiedadesModel>)result.Model;
                return View(propiedades);
            }
            return View();
        }
    }
}
