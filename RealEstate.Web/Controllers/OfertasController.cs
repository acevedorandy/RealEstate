using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Contracts.dbo;
using RealEstate.Application.Dtos.dbo;
using RealEstate.Application.Services.dbo;
using RealEstate.Persistance.Models.dbo;
using RealEstate.Persistance.Models.ViewModel;

namespace RealEstate.Web.Controllers
{
    public class OfertasController : Controller
    {
        private readonly IOfertasService _ofertasService;

        public OfertasController(IOfertasService ofertasService)
        {
            _ofertasService = ofertasService;
        }

        public async Task <IActionResult> Index()
        {
            var result = await _ofertasService.GetPropertyOfferedAsync();

            if (result.IsSuccess)
            {
                List<OfertasViewModel> ofertas = (List<OfertasViewModel>)result.Model;
                return View(ofertas);
            }
            return View();
        }

        public async Task <IActionResult> Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OfertasDto dto)
        {
            try
            {
                var existeRelacion = await _ofertasService.PendingBidsAsync();

                if (existeRelacion)
                {
                    TempData["ErrorMessage"] = "Al parecer esta usted ya poseé una oferta en estado pendiente.";
                    return RedirectToAction("Index", "Clientes");
                }

                var result = await _ofertasService.SaveAsync(dto);

                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Oferta enviada exitosamente.";
                    return RedirectToAction("Index", "Clientes");
                }
                else
                {
                    TempData["ErrorMessage"] = result.Messages;
                    return View(dto);
                }
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


    }
}
