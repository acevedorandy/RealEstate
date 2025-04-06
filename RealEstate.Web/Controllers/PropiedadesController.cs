using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Contracts.dbo;
using RealEstate.Application.Dtos.dbo;
using RealEstate.Persistance.Models.dbo;

namespace RealEstate.Web.Controllers
{
    public class PropiedadesController : Controller
    {
        private readonly IPropiedadesService _propiedadesService;

        public PropiedadesController(IPropiedadesService propiedadesService)
        {
            _propiedadesService = propiedadesService;
        }

        public async Task <IActionResult> Index()
        {
            var result = await _propiedadesService.GetAllAsync();

            if (result.IsSuccess)
            {
                List<PropiedadesModel> propiedades = (List<PropiedadesModel>)result.Model;
                return View(propiedades);
            }
            return View();
        }

        public async Task <IActionResult> Details(int id)
        {
            var result = await _propiedadesService.GetByIDAsync(id);

            if (result.IsSuccess)
            {
                PropiedadesModel propiedades = (PropiedadesModel)result.Model;
                return View(propiedades);
            }
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Create(PropiedadesDto dto)
        {
            try
            {
                var result = await _propiedadesService.SaveAsync(dto);

                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Propiedad agregada exitosamente.";
                    return RedirectToAction(nameof(Index));
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

        public async Task <IActionResult> Edit(int id)
        {
            var result = await _propiedadesService.GetByIDAsync(id);

            if (result.IsSuccess)
            {
                PropiedadesModel propiedades = (PropiedadesModel)result.Model;
                return View(propiedades);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Edit(PropiedadesDto dto)
        {
            try
            {
                var result = await _propiedadesService.UpdateAsync(dto);

                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Propiedad editada exitosamente.";
                    return RedirectToAction(nameof(Index));
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

        public async Task <IActionResult> Delete(int id)
        {
            var result = await _propiedadesService.GetByIDAsync(id);

            if (result.IsSuccess)
            {
                PropiedadesModel propiedades = (PropiedadesModel)result.Model;
                return View(propiedades);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Delete(PropiedadesDto dto)
        {
            try
            {
                var result = await _propiedadesService.RemoveAsync(dto);

                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Propiedad eliminada exitosamente.";
                    return RedirectToAction(nameof(Index));
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
    }
}
