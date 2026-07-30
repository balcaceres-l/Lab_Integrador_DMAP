using lab_integrador.Data;
using lab_integrador.Models;

namespace lab_integrador.Repository
{
    public class ManufacturingProcessRepository : IManufacturingProcessRepository
    {
        private readonly ApplicationDbContext _context;

        public ManufacturingProcessRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<ManufacturingProcess> GetAll()
        {
            return _context.ManufacturingProcesses
                .OrderBy(p => p.Name)
                .ToList();
        }
        public ManufacturingProcess? GetById(int id)
        {
            return _context.ManufacturingProcesses.Find(id);
        }
        public void Add(ManufacturingProcess process)
        {
            _context.ManufacturingProcesses.Add(process);
            _context.SaveChanges();
        }

        public void Update(ManufacturingProcess process)
        {
            _context.ManufacturingProcesses.Update(process);
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            var process = _context.ManufacturingProcesses.Find(id);
            if (process != null)
            {
                _context.ManufacturingProcesses.Remove(process);
                _context.SaveChanges();
            }
        }
        public bool ExistsByName(string name, int? excludeId = null)
        {
            return _context.ManufacturingProcesses
                .Any(p => p.Name.ToLower() == name.ToLower() && p.Id != excludeId);
        }
        public bool IsInUse(int id)
        {
            return _context.OrderProcesses.Any(op => op.ManufacturingProcessId == id);
        }
    }
}
