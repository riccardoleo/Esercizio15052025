namespace Esercizio15052025.DTO.ToolCategory_DTO;

public class ToolCategoryDTO_Response
{
    /// <summary>
    /// [200] OK / [204] No Content / [404] Not found / [500] Internal Server Error
    /// </summary>
    public int success { get; set; }
    public int? UserId { get; set; }
    public TC_DTO? toolCategory_DTO { get; set; }
    public List<TC_DTO>? toolCategories { get; set; }
    public string? message { get; set; }
    
}