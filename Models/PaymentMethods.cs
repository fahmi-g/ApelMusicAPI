namespace ApelMusicAPI.Models
{
    public class PaymentMethods
    {
        public int paymentId { get; set; }
        public string paymentName { get; set; } = string.Empty;
        public string paymentImg { get; set; } = string.Empty;
        public bool isActive { get; set; }
    }
}
