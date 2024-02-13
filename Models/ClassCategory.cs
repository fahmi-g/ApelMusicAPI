namespace ApelMusicAPI.Models
{
    public class ClassCategory
    {
        public int categoryId { get; set; }
        public string? categoryImg { get; set; }
        public string categoryName { get; set; } = string.Empty;
        public string categoryDescription { get; set; } = string.Empty;
        public bool isActive { get; set; }
    }
}
