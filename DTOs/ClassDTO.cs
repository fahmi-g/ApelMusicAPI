using ApelMusicAPI.Models;

namespace ApelMusicAPI.DTOs
{
    public class ClassDTO
    {
        public int classCategory { get; set; }
        public string? classImg { get; set; }
        public string className { get; set; } = string.Empty;
        public string? classDescription { get; set; }
        public int classPrice { get; set; }
        public bool isActive { get; set; }

        public List<DateTime> classSchedules { get; set; }
    }
}
