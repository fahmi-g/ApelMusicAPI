namespace ApelMusicAPI.DTOs
{
    public class CheckoutDTO
    {
        public Guid orderBy { get; set; }
        public string paymentMethod { get; set; } = string.Empty;
        public int[] selectedClasses { get; }
    }
}
