using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Contracts.dbo;
using RealEstate.Application.Dtos.dbo;
using RealEstate.Persistance.Models.dbo;

namespace RealEstate.Web.Controllers
{
    public class ContratosController : Controller
    {
        private readonly IContratosService _contratosService;

        public ContratosController(IContratosService contratosService)
        {
            _contratosService = contratosService;
        }

        public async Task <IActionResult> Index()
        {
            var result = await _contratosService.GetAllAsync();

            if (result.IsSuccess)
            {
                List<ContratosModel> contratosModels = (List<ContratosModel>)result.Model;
                return View(contratosModels);
            }
            return View();
        }

        public async Task <IActionResult> Details(int id)
        {
            var result = await _contratosService.GetByIDAsync(id);

            if (result.IsSuccess)
            {
                ContratosModel contratosModel = (ContratosModel)result.Model;
                return View(contratosModel);
            }
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Create(ContratosDto dto)
        {
            try
            {
                var result = await _contratosService.SaveAsync(dto);

                if (result.IsSuccess) 
                {
                    TempData["SuccessMessage"] = "Contrato registrado exitosamente.";
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
            var result = await _contratosService.GetByIDAsync(id);

            if (result.IsSuccess)
            {
                ContratosModel contratosModel = (ContratosModel)result.Model;
                return View(contratosModel);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Edit(ContratosDto dto)
        {
            try
            {
                var result = await _contratosService.UpdateAsync(dto);

                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Contrato actualizado exitosamente.";
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
            var result = await _contratosService.GetByIDAsync(id);

            if (result.IsSuccess)
            {
                ContratosModel contratosModel = (ContratosModel)result.Model;
                return View(contratosModel);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Delete(ContratosDto dto)
        {
            try
            {
                var result = await _contratosService.RemoveAsync(dto);

                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Contrato eliminado exitosamente.";
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
