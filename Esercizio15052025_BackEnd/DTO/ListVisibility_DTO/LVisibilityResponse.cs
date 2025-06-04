namespace Esercizio20052025.DTO.ListVisibility_DTO
{
    public class LVisibilityResponse
    {
        /// <summary>
        /// [200] OK / [204] No Content / [404] Not found / [500] Internal Server Error
        /// </summary>
        public int success { get; set; }
        public int PermissionId { get; set; }
        public string? message { get; set; }
        public List<ListVisibility_DTO> listVisibility_DTOs { get; set; }
        public List<int> PermissionIdList { get; set; }

    }
}
