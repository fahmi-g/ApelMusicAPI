namespace ApelMusicAPI.DTOs
{
    public class LoginResponseDTO
    {
        public Guid userId { get; set; }
        public string token {  get; set; } = string.Empty;
        public string role {  get; set; } = string.Empty;
    }
}
