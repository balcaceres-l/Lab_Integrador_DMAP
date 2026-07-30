using lab_integrador.Data;
using lab_integrador.Models;
using Microsoft.EntityFrameworkCore;
namespace lab_integrador.Repository
{
    public class ProductionOrderRepository : IProductionOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductionOrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ProductionOrder> GetAll()
        {
            return _context.ProductionOrders
                .Include(o => o.OrderProcesses)
                .OrderByDescending(o => o.OrderDate)
                .ToList();
        }

        public ProductionOrder? GetById(int id)
        {
            return _context.ProductionOrders.Find(id);
        }

        public ProductionOrder? GetByIdWithProcesses(int id)
        {
            return _context.ProductionOrders
                .Include(o => o.OrderProcesses)
                    .ThenInclude(op => op.ManufacturingProcess)
                .FirstOrDefault(o => o.Id == id);
        }

        public void Add(ProductionOrder order, IEnumerable<int> processIds)
        {
            foreach (var processId in processIds.Distinct())
            {
                order.OrderProcesses.Add(new OrderProcess
                {
                    ManufacturingProcessId = processId,
                    IsCompleted = false
                });
            }

            _context.ProductionOrders.Add(order);
            _context.SaveChanges();
        }

        public void Update(ProductionOrder order, IEnumerable<int> processIds)
        {
            var existingOrder = _context.ProductionOrders
                .Include(o => o.OrderProcesses)
                .FirstOrDefault(o => o.Id == order.Id);

            if (existingOrder == null) return;
            existingOrder.ProductName = order.ProductName;
            existingOrder.Quantity = order.Quantity;
            existingOrder.OrderDate = order.OrderDate;

            var newProcessIds = processIds.Distinct().ToList();
            var toRemove = existingOrder.OrderProcesses
                .Where(op => !newProcessIds.Contains(op.ManufacturingProcessId))
                .ToList();
            foreach (var op in toRemove)
                existingOrder.OrderProcesses.Remove(op);
            var currentProcessIds = existingOrder.OrderProcesses.Select(op => op.ManufacturingProcessId);
            var toAdd = newProcessIds.Except(currentProcessIds);
            foreach (var processId in toAdd)
            {
                existingOrder.OrderProcesses.Add(new OrderProcess
                {
                    ProductionOrderId = existingOrder.Id,
                    ManufacturingProcessId = processId,
                    IsCompleted = false
                });
            }

            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            var order = _context.ProductionOrders.Find(id);
            if (order != null)
            {
                _context.ProductionOrders.Remove(order);
                _context.SaveChanges();
            }
        }

        public void SetProcessCompletion(int orderId, int processId, bool isCompleted)
        {
            var orderProcess = _context.OrderProcesses
                .FirstOrDefault(op => op.ProductionOrderId == orderId && op.ManufacturingProcessId == processId);

            if (orderProcess != null)
            {
                orderProcess.IsCompleted = isCompleted;
                orderProcess.CompletedDate = isCompleted ? DateTime.Now : null;
                _context.SaveChanges();
            }
        }
    }
}
