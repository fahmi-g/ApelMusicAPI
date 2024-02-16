namespace ApelMusicAPI.DTOs
{
    public class EditAccountDTO
    {
        public string userName { get; set; } = string.Empty;
        public string userEmail { get; set; } = string.Empty;
        public int role { get; set; }
        public bool isActivated { get; set; }
    }
}
