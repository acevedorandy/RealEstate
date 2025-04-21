using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Contracts.dbo;
using RealEstate.Application.Dtos.dbo;
using RealEstate.Persistance.Models.dbo;

namespace RealEstate.Web.Controllers
{
    public class TiposVentaController : Controller
    {
        private readonly ITiposVentaService _tiposVentaService;

        public TiposVentaController(ITiposVentaService tiposVentaService)
        {
            _tiposVentaService = tiposVentaService;
        }

        public async Task <IActionResult> Index()
        {
            var result = await _tiposVentaService.GetAllAsync();

            if (result.IsSuccess)
            {
                List<TiposVentaModel> tiposVentas = (List<TiposVentaModel>)result.Model;
                return View(tiposVentas);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Create(TiposVentaDto dto)
        {
            try
            {
                var result = await _tiposVentaService.SaveAsync(dto);

                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Tipo de venta guardado exitosamente.";
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
        public async Task <IActionResult> Edit(TiposVentaDto dto)
        {
            try
            {
                var result = await _tiposVentaService.UpdateAsync(dto);

                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Tipo de venta actualizado exitosamente.";
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
        public async Task <IActionResult> Delete(TiposVentaDto dto)
        {
            try
            {
                var result = await _tiposVentaService.RemoveAsync(dto);

                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Tipo de venta y sus propiedades eliminado exitosamente.";
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
