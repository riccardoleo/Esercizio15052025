using Esercizio15052025.DTO.Tool_DTO;

namespace Esercizio20052025.DTO.Tool_DTO
{
    public class ToolDTO_Response
    {
        /// <summary>
        /// [200] OK / [204] No Content / [404] Not found / [500] Internal Server Error
        /// </summary>
        public int success;
        public int? UserId;
        public T_DTO? tool_DTO;
        public List<T_DTO>? tools;
        public string? message;
    }
}
