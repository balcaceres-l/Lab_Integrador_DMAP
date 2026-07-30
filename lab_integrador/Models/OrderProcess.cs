using System.ComponentModel.DataAnnotations;

namespace lab_integrador.Models
{
    public class OrderProcess
    {
        public int ProductionOrderId { get; set; }
        public ProductionOrder ProductionOrder { get; set; } = null!;

        public int ManufacturingProcessId { get; set; }
        public ManufacturingProcess ManufacturingProcess { get; set; } = null!;

        [Display(Name = "Completado")]
        public bool IsCompleted { get; set; } = false;

        [Display(Name = "Fecha de finalización")]
        public DateTime? CompletedDate { get; set; }
    }
}
