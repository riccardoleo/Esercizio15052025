using System.ComponentModel.DataAnnotations;

namespace Esercizio15052025.DTO.ToolCategory_DTO
{
    public class TC_DTO
    {
        [Required(ErrorMessage = "Il campo 'Name' è obbligatorio.")]
        public string Name { get; set; }
        public int? CategoryId { get; set; }
        public int? CreatedByUserId { get; set; }
    }
}
