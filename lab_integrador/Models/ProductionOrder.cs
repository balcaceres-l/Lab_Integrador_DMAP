using System.ComponentModel.DataAnnotations;

namespace lab_integrador.Models
{
    public class ProductionOrder
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [StringLength(150, ErrorMessage = "El nombre no puede exceder los 150 caracteres.")]
        [Display(Name = "Nombre del producto")]
        public string ProductName { get; set; } = string.Empty;

        [Required(ErrorMessage = "La cantidad a producir es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor que cero.")]
        [Display(Name = "Cantidad a producir")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "La fecha de la orden es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de orden")]
        public DateTime OrderDate { get; set; } = DateTime.Today;

        public ICollection<OrderProcess> OrderProcesses { get; set; } = new List<OrderProcess>();

        [Display(Name = "Estado")]
        public string Status
        {
            get
            {
                if (OrderProcesses == null || !OrderProcesses.Any())
                    return "Sin procesos";

                int total = OrderProcesses.Count;
                int completed = OrderProcesses.Count(op => op.IsCompleted);

                if (completed == 0) return "Pendiente";
                if (completed == total) return "Completada";
                return $"En proceso ({completed}/{total})";
            }
        }
    }
}
