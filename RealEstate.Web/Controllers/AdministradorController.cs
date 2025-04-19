using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Contracts.dbo;
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

        //public async Task<IActionResult> ObtenerAgentes()
        //{
        //    var result = await _usuariosService.GetIdentityUserAllAsync();

        //    if (result.IsSuccess)
        //    {
        //        List<AgentesM>
        //    }
        //}

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
