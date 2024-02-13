namespace ApelMusicAPI.DTOs
{
    public class ClassCategoryDTO
    {
        public string? categoryImg { get; set; }
        public string categoryName { get; set; } = string.Empty;
        public string categoryDescription { get; set; } = string.Empty;
        public bool isActive { get; set; }
    }
}
