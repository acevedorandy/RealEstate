using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Contracts.dbo;
using RealEstate.Persistance.Models.dbo;
using RealEstate.Web.Middlewares;

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

        [ServiceFilter(typeof(LoginAuthorize))]
        [Authorize(Roles = "Cliente")]
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

        [ServiceFilter(typeof(LoginAuthorize))]
        [Authorize(Roles = "Cliente")]
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

        [ServiceFilter(typeof(LoginAuthorize))]
        [Authorize(Roles = "Cliente")]
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

        [ServiceFilter(typeof(LoginAuthorize))]
        [Authorize(Roles = "Agente")]
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
