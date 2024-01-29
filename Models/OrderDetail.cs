namespace ApelMusicAPI.Models
{
    public class OrderDetail
    {
        public int orderDetailId { get; set; }
        public string invoiceNo { get; set; } = string.Empty;
        public int classId { get; set; }
    }
}
