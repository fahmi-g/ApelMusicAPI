namespace ApelMusicAPI.DTOs
{
    public class BuyNowDTO
    {
        public Guid userId { get; set; }
        public int classId { get; set; }
        public DateTime classSchedule { get; set; }
        public string paymentMethod { get; set; } = string.Empty;
    }
}
