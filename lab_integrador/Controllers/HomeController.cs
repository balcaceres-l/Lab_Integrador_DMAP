using lab_integrador.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using lab_integrador.Repository;
namespace lab_integrador.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductionOrderRepository _orderRepository;
        private readonly IManufacturingProcessRepository _processRepository;
        public HomeController(
            IProductionOrderRepository orderRepository,
            IManufacturingProcessRepository processRepository)
        {
            _orderRepository = orderRepository;
            _processRepository = processRepository;
        }
        public IActionResult Index()
        {
            var orders = _orderRepository.GetAll().ToList();

            ViewBag.TotalOrders = orders.Count;
            ViewBag.CompletedOrders = orders.Count(o => o.Status == "Completada");
            ViewBag.PendingOrders = orders.Count(o => o.Status == "Pendiente");
            ViewBag.TotalProcesses = _processRepository.GetAll().Count();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
