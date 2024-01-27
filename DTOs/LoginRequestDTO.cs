namespace ApelMusicAPI.DTOs
{
    public class LoginRequestDTO
    {
        public string userName { get; set; } = string.Empty;
        public string userEmail { get; set; } = string.Empty;
        public string userPassword { get; set; } = string.Empty;
    }
}
