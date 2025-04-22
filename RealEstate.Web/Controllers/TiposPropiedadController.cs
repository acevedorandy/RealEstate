using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Contracts.dbo;
using RealEstate.Application.Dtos.dbo;
using RealEstate.Persistance.Models.dbo;

namespace RealEstate.Web.Controllers
{
    public class TiposPropiedadController : Controller
    {
        private readonly ITiposPropiedadService _tiposPropiedadService;

        public TiposPropiedadController(ITiposPropiedadService tiposPropiedadService)
        {
            _tiposPropiedadService = tiposPropiedadService;
        }

        public async Task <IActionResult> Index()
        {
            var result = await _tiposPropiedadService.GetAllAsync();

            if (result.IsSuccess)
            {
                List<TiposPropiedadModel> tiposPropiedads = (List<TiposPropiedadModel>)result.Model;
                return View(tiposPropiedads);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Create(TiposPropiedadDto dto)
        {
            try
            {
                var result = await _tiposPropiedadService.SaveAsync(dto);

                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Tipo de propiedad guardado exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = result.Messages;
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Edit(TiposPropiedadDto dto)
        {
            try
            {
                var result = await _tiposPropiedadService.UpdateAsync(dto);

                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Tipo de propiedad actualizado exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = result.Messages;
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Delete(TiposPropiedadDto dto)
        {
            try
            {
                var result = await _tiposPropiedadService.RemoveTypeWithPropertyAsync(dto.TipoPropiedadID);

                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Tipo de propiedad y las propiedades asociadas eliminada exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = result.Messages;
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                return View();
            }
        }
    }
}
