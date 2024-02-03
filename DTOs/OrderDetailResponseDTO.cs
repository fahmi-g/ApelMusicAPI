namespace ApelMusicAPI.DTOs
{
    public class OrderDetailResponseDTO
    {
        public string className { get; set; } = string.Empty;
        public string category {  get; set; } = string.Empty;
        public DateTime schedule {  get; set; }
        public int classPrice { get; set; }
    }
}
