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
            return View(_repository.GetAll());
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
            if (ModelState.IsValid)
            {
                if (_repository.ExistsByName(process.Name))
                {
                    ModelState.AddModelError("Name", "Ya existe un proceso con este nombre.");
                    return View(process);
                }

                _repository.Add(process);
                TempData["SuccessMessage"] = "Proceso de manufactura creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            return View(process);
        }
    }
}
