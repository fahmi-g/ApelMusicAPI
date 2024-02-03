namespace ApelMusicAPI.Models
{
    public class Orders
    {
        public Guid orderId {  get; set; }
        public string invoiceNo { get; set; } = string.Empty;
        public DateTime orderDate { get; set; }
        public Guid orderBy { get; set; }
        public string paymentMethod { get; set; } = string.Empty;
        public bool isPaid { get; set; }
        public int totalPrice { get; set; }
    }
}
