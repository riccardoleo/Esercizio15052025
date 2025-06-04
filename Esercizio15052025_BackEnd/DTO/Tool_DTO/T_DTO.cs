using System.ComponentModel.DataAnnotations;

namespace Esercizio15052025.DTO.Tool_DTO
{
    public class T_DTO
    {
        [Required(ErrorMessage = "Il campo 'Name' è obbligatorio.")]
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ToolId { get; set; }
        public DateTime CreationDate { get; set; }
        public int CategoryId { get; set; }
        public int PlantComponentId { get; set; }
        public int? CreatedByUserId { get; set; }
    }
}
