namespace Esercizio15052025.DTO.Tool_DTO
{
    public class T_DTO_Update
    {
        public int ToolId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public int? CreatedByUserId { get; set; }
    }
}
