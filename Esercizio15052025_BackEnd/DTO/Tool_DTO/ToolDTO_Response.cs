using Esercizio15052025.DTO.Tool_DTO;

namespace Esercizio20052025.DTO.Tool_DTO
{
    public class ToolDTO_Response
    {
        /// <summary>
        /// [200] OK / [204] No Content / [404] Not found / [500] Internal Server Error
        /// </summary>
        public int success { get; set; }
        public int? UserId { get; set; }
        public T_DTO? tool_DTO { get; set; }
        public List<T_DTO>? tools { get; set; }
        public string? message { get; set; }
    }
}
