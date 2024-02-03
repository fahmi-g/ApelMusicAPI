namespace ApelMusicAPI.DTOs
{
    public class OrderResponseDTO
    {
        public string invoiceNo { get; set; } = string.Empty;
        public DateTime orderDate { get; set; }
        public int totalClasses { get; set; }
        public int totalPrice { get; set; }
    }
}
