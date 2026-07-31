using lab_integrador.Models;
using lab_integrador.Repository;
using Microsoft.AspNetCore.Mvc;

namespace lab_integrador.Controllers
{
    public class ProductionOrdersController : Controller
    {
        private readonly IProductionOrderRepository _orderRepository;
        private readonly IManufacturingProcessRepository _processRepository;

        public ProductionOrdersController(
            IProductionOrderRepository orderRepository,
            IManufacturingProcessRepository processRepository)
        {
            _orderRepository = orderRepository;
            _processRepository = processRepository;
        }
        public IActionResult Index()
        {
            var orders = _orderRepository.GetAll();
            return View(orders);
        }

        // GET: ProductionOrders/Details/5
        public IActionResult Details(int id)
        {
            var order = _orderRepository.GetByIdWithProcesses(id);
            if (order == null) return NotFound();

            return View(order);
        }

        // GET: ProductionOrders/Create
        public IActionResult Create()
        {
            var viewModel = new ProductionOrderViewModel
            {
                AvailableProcesses = _processRepository.GetAll()
                    .Select(p => new ProcessCheckboxItem
                    {
                        ProcessId = p.Id,
                        Name = p.Name,
                        IsSelected = false
                    }).ToList()
            };

            return View(viewModel);
        }

        // POST: ProductionOrders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductionOrderViewModel viewModel)
        {
            ValidateAtLeastOneProcess(viewModel.SelectedProcessIds);

            if (!ModelState.IsValid)
            {
                ReloadAvailableProcesses(viewModel);
                return View(viewModel);
            }

            var order = new ProductionOrder
            {
                ProductName = viewModel.ProductName,
                Quantity = viewModel.Quantity,
                OrderDate = viewModel.OrderDate
            };

            _orderRepository.Add(order, viewModel.SelectedProcessIds);
            TempData["SuccessMessage"] = "Orden de producción creada exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        // GET: ProductionOrders/Edit/5
        public IActionResult Edit(int id)
        {
            var order = _orderRepository.GetByIdWithProcesses(id);
            if (order == null) return NotFound();

            var selectedIds = order.OrderProcesses.Select(op => op.ManufacturingProcessId).ToList();

            var viewModel = new ProductionOrderViewModel
            {
                Id = order.Id,
                ProductName = order.ProductName,
                Quantity = order.Quantity,
                OrderDate = order.OrderDate,
                SelectedProcessIds = selectedIds,
                AvailableProcesses = _processRepository.GetAll()
                    .Select(p => new ProcessCheckboxItem
                    {
                        ProcessId = p.Id,
                        Name = p.Name,
                        IsSelected = selectedIds.Contains(p.Id)
                    }).ToList()
            };

            return View(viewModel);
        }

        // POST: ProductionOrders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, ProductionOrderViewModel viewModel)
        {
            if (id != viewModel.Id) return NotFound();

            ValidateAtLeastOneProcess(viewModel.SelectedProcessIds);

            if (!ModelState.IsValid)
            {
                ReloadAvailableProcesses(viewModel);
                return View(viewModel);
            }

            var order = new ProductionOrder
            {
                Id = viewModel.Id,
                ProductName = viewModel.ProductName,
                Quantity = viewModel.Quantity,
                OrderDate = viewModel.OrderDate
            };

            _orderRepository.Update(order, viewModel.SelectedProcessIds);
            TempData["SuccessMessage"] = "Orden de producción actualizada exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        // GET: ProductionOrders/Delete/5
        public IActionResult Delete(int id)
        {
            var order = _orderRepository.GetByIdWithProcesses(id);
            if (order == null) return NotFound();

            return View(order);
        }

        // POST: ProductionOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _orderRepository.Delete(id);
            TempData["SuccessMessage"] = "Orden de producción eliminada exitosamente.";
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleProcessCompletion(int orderId, int processId, bool isCompleted)
        {
            _orderRepository.SetProcessCompletion(orderId, processId, isCompleted);
            TempData["SuccessMessage"] = "Estado del proceso actualizado.";
            return RedirectToAction(nameof(Details), new { id = orderId });
        }
        private void ValidateAtLeastOneProcess(List<int> selectedProcessIds)
        {
            if (selectedProcessIds == null || !selectedProcessIds.Any())
            {
                ModelState.AddModelError(nameof(ProductionOrderViewModel.SelectedProcessIds),
                    "La orden debe tener al menos un proceso asociado.");
            }
        }
        private void ReloadAvailableProcesses(ProductionOrderViewModel viewModel)
        {
            viewModel.AvailableProcesses = _processRepository.GetAll()
                .Select(p => new ProcessCheckboxItem
                {
                    ProcessId = p.Id,
                    Name = p.Name,
                    IsSelected = viewModel.SelectedProcessIds.Contains(p.Id)
                }).ToList();
        }
    }
}
