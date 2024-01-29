namespace ApelMusicAPI.DTOs
{
    public class OrdersDTO
    {
        public Guid orderBy { get; set; }
        public string paymentMethod { get; set; } = string.Empty;
    }
}
