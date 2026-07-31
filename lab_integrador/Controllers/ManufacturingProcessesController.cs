using lab_integrador.Models;
using lab_integrador.Repository;
using Microsoft.AspNetCore.Mvc;

namespace lab_integrador.Controllers
{
    public class ManufacturingProcessesController : Controller
    {
        private readonly IManufacturingProcessRepository _repository;

        public ManufacturingProcessesController(IManufacturingProcessRepository repository)
        {
            _repository = repository;
        }

        // GET: ManufacturingProcesses
        public IActionResult Index()
        {
            var processes = _repository.GetAll();
            return View(processes);
        }

        // GET: ManufacturingProcesses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ManufacturingProcesses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ManufacturingProcess process)
        {
            if (_repository.ExistsByName(process.Name))
            {
                ModelState.AddModelError(nameof(process.Name), "Ya existe un proceso con este nombre.");
            }

            if (!ModelState.IsValid)
            {
                return View(process);
            }

            _repository.Add(process);
            TempData["SuccessMessage"] = "Proceso de fabricación creado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        // GET: ManufacturingProcesses/Edit/5
        public IActionResult Edit(int id)
        {
            var process = _repository.GetById(id);
            if (process == null) return NotFound();

            return View(process);
        }

        // POST: ManufacturingProcesses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, ManufacturingProcess process)
        {
            if (id != process.Id) return NotFound();

            if (_repository.ExistsByName(process.Name, process.Id))
            {
                ModelState.AddModelError(nameof(process.Name), "Ya existe otro proceso con este nombre.");
            }

            if (!ModelState.IsValid)
            {
                return View(process);
            }

            _repository.Update(process);
            TempData["SuccessMessage"] = "Proceso de fabricación actualizado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        // GET: ManufacturingProcesses/Delete/5
        public IActionResult Delete(int id)
        {
            var process = _repository.GetById(id);
            if (process == null) return NotFound();

            ViewBag.IsInUse = _repository.IsInUse(id);
            return View(process);
        }

        // POST: ManufacturingProcesses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (_repository.IsInUse(id))
            {
                TempData["ErrorMessage"] = "No se puede eliminar el proceso porque está asociado a una o más órdenes de producción.";
                return RedirectToAction(nameof(Index));
            }

            _repository.Delete(id);
            TempData["SuccessMessage"] = "Proceso de fabricación eliminado exitosamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}