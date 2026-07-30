using System.ComponentModel.DataAnnotations;

namespace lab_integrador.Models
{
    public class ManufacturingProcess
    {
            public int Id { get; set; }

            [Required(ErrorMessage = "El nombre del proceso es obligatorio.")]
            [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
            [Display(Name = "Nombre del proceso")]
            public string Name { get; set; } = string.Empty;

            [StringLength(300, ErrorMessage = "La descripción no puede exceder los 300 caracteres.")]
            [Display(Name = "Descripción")]
            public string? Description { get; set; }
            public ICollection<OrderProcess> OrderProcesses { get; set; } = new List<OrderProcess>();
        }
    
}
