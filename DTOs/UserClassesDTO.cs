namespace ApelMusicAPI.DTOs
{
    public class UserClassesDTO
    {
        public Guid userId { get; set; }
        public int classId { get; set; }
        public int scheduleId { get; set; }
        public DateTime classSchedule { get; set; }
    }
}
