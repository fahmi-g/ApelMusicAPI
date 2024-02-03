namespace ApelMusicAPI.Models
{
    public class Class
    {
        public int classId { get; set; }
        public int classCategory { get; set; }
        public string? classImg { get; set; }
        public string className { get; set; } = string.Empty;
        public string? classDescription { get; set; }
        public int classPrice { get; set; }
        public string classStatus { get; set; } = string.Empty;
        public string categoryName { get; set; } = string.Empty;
    }
}
