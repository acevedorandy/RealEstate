using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Contracts.dbo;
using RealEstate.Application.Dtos;
using RealEstate.Persistance.Models.dbo;

namespace RealEstate.Web.Controllers
{
    public class PropiedadTiposVentaController : Controller
    {
        private readonly IPropiedadTiposVentaService _propiedadTiposVenta;

        public PropiedadTiposVentaController(IPropiedadTiposVentaService propiedadTiposVenta)
        {
            _propiedadTiposVenta = propiedadTiposVenta;
        }

        public async Task <IActionResult> Index()
        {
            var result = await _propiedadTiposVenta.GetAllAsync();

            if (result.IsSuccess)
            {
                List<PropiedadTiposVentaModel> ventaModels = (List<PropiedadTiposVentaModel>)result.Model;
                return View(ventaModels);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Create(PropiedadTiposVentaDto dto)
        {
            try
            {
                var result = await _propiedadTiposVenta.SaveAsync(dto);

                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Tipo de venta guardada exitosamente.";
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
        public async Task <IActionResult> Edit(PropiedadTiposVentaDto dto)
        {
            try
            {
                var result = await _propiedadTiposVenta.UpdateAsync(dto);

                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Tipo de venta actualizada exitosamente.";
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
        public async Task <IActionResult> Delete(PropiedadTiposVentaDto dto)
        {
            try
            {
                var result = await _propiedadTiposVenta.RemoveAsync(dto);

                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Tipo de venta eliminado exitosamente.";
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
