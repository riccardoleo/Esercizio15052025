using Esercizio15052025.DTO.PlantComponent_DTO;

namespace Esercizio20052025.DTO.PlantComponent_DTO
{
    public class PlantComponent_Response
    {
        /// <summary>
        /// [200] OK / [204] No Content / [404] Not found / [500] Internal Server Error
        /// </summary>
        public int success {  get; set; }
        public int? PC_ID { get; set; }
        public PC_DTO? PC_DTO { get; set; }
        public List<PC_DTO>? List_PC_DTO { get; set; }
        public string? message { get; set; }
    }
}
