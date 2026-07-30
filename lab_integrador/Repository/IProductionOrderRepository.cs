using lab_integrador.Models;

namespace lab_integrador.Repository
{
    public interface IProductionOrderRepository
    {
        IEnumerable<ProductionOrder> GetAll();
        ProductionOrder? GetById(int id);
        ProductionOrder? GetByIdWithProcesses(int id);
        void Add(ProductionOrder order, IEnumerable<int> processIds);
        void Update(ProductionOrder order, IEnumerable<int> processIds);
        void Delete(int id);
        void SetProcessCompletion(int orderId, int processId, bool isCompleted);
    }
}
