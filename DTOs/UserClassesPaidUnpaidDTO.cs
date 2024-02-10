namespace ApelMusicAPI.DTOs
{
    public class UserClassesPaidUnpaidDTO
    {
        public int userClassId {  get; set; }
        public int classId { get; set; }
        public int classCategory { get; set; }
        public string categoryName { get; set; } = string.Empty;
        public string? classImg { get; set; }
        public string className { get; set; } = string.Empty;
        public string? classDescription { get; set; }
        public int classPrice { get; set; }
        public string classStatus { get; set; } = string.Empty;
        public DateTime classSchedule { get; set; }
    }
}
