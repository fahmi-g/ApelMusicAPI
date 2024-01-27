using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using ApelMusicAPI.Models;
using ApelMusicAPI.DTOs;
using ApelMusicAPI.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MySql.Data.MySqlClient;

namespace ApelMusicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserData userData;
        private readonly IConfiguration _configuration;

        public UserController(UserData userData, IConfiguration configuration)
        {
            this.userData = userData;
            _configuration = configuration;
        }


        [HttpPost("CreateAccount")]
        public IActionResult CreateAccount([FromBody] UserDTO userDTO)
        {
            try
            {
                User newUser = new User
                {
                    userId = Guid.NewGuid(),
                    userName = userDTO.userName,
                    userEmail = userDTO.userEmail,
                    userPassword = BCrypt.Net.BCrypt.HashPassword(userDTO.userPassword)
                };

                bool result = userData.CreateAccount(newUser);

                if (result)
                {
                    return StatusCode(201, $"{userDTO.userName}, your account has been created.");
                }
                else
                {
                    return StatusCode(500, "Account creation failed.");
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDTO credential)
        {
            if (credential is null)
                return BadRequest("Invalid client request");

            if (string.IsNullOrEmpty(credential.userName) || string.IsNullOrEmpty(credential.userPassword))
                return BadRequest("Invalid client request");

            User? user = userData.CheckUserAuth(credential.userName);

            if (user == null)
                return Unauthorized("You do not authorized");

            string? userRole = userData.GetRoleNameById(user.role);

            bool isVerified = BCrypt.Net.BCrypt.Verify(credential.userPassword, user?.userPassword);
            //bool isVerified = user?.Password == credential.Password;

            if (user != null && !isVerified)
            {
                return BadRequest("Incorrect Password! Please check your password!");
            }
            else
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("JwtConfig:Key").Value));

                var claims = new Claim[] {
                    new Claim(ClaimTypes.Name, user.userName),
                    new Claim(ClaimTypes.Role, userRole)
                };

                var signingCredential = new SigningCredentials(
                    key, SecurityAlgorithms.HmacSha256Signature);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddMinutes(10),
                    SigningCredentials = signingCredential
                };

                var tokenHandler = new JwtSecurityTokenHandler();

                var securityToken = tokenHandler.CreateToken(tokenDescriptor);

                string token = tokenHandler.WriteToken(securityToken);

                return Ok(new LoginResponseDTO { token = token });
            }
        }
    }
}
