using lab_integrador.Models;

namespace lab_integrador.Repository
{
    public interface IManufacturingProcessRepository
    {
        IEnumerable<ManufacturingProcess> GetAll();
        ManufacturingProcess? GetById(int id);
        void Add(ManufacturingProcess process);
        void Update(ManufacturingProcess process);
        void Delete(int id);
        bool ExistsByName(string name, int? excludeId = null);
        bool IsInUse(int id); 
    }
}
