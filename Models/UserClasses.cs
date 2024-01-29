namespace ApelMusicAPI.Models
{
    public class UserClasses
    {
        public int userClassesId {  get; set; }
        public Guid userId { get; set; }
        public int classId { get; set; }
        public DateTime classSchedule {  get; set; }
        public bool isPaid { get; set; }
    }
}
