using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Contracts.dbo;
using RealEstate.Application.Dtos.dbo;
using RealEstate.Persistance.Models.ViewModel;
using RealEstate.Web.Middlewares;

namespace RealEstate.Web.Controllers
{
    public class OfertasController : Controller
    {
        private readonly IOfertasService _ofertasService;
        private readonly IPropiedadesService _propiedadesService;

        public OfertasController(IOfertasService ofertasService,
                                 IPropiedadesService propiedadesService)
        {
            _ofertasService = ofertasService;
            _propiedadesService = propiedadesService;
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        [Authorize(Roles = "Cliente")]
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

        [ServiceFilter(typeof(LoginAuthorize))]
        [Authorize(Roles = "Cliente")]
        public ActionResult Create()
        {
            return View();
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        [Authorize(Roles = "Cliente")]
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

        [ServiceFilter(typeof(LoginAuthorize))]
        [Authorize(Roles = "Agente")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarOrRechazarOferta(OfertasDto dto)
        {
            try
            {
                var result = await _ofertasService.UpdateAsync(dto);

                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = "No se pudo actualizar la oferta." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error en el servidor." });
            }
        }
    }
}
