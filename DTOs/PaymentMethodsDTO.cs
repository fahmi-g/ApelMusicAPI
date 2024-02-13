namespace ApelMusicAPI.DTOs
{
    public class PaymentMethodsDTO
    {
        public string paymentName { get; set; } = string.Empty;
        public string paymentImg { get; set; } = string.Empty;
        public bool isActive { get; set; }
    }
}
