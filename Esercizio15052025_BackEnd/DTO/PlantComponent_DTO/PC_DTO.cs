using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Esercizio15052025.DTO.PlantComponent_DTO
{
    public class PC_DTO
    {
        [Required(ErrorMessage = "Il campo 'Name' è obbligatorio.")]
        public required string Name { get; set; }
        public int? ComponentId { get; set; }
        public string Description { get; set; }
        public int? CreatedByUserId { get; set; }
    }
}
