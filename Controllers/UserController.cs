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
using Microsoft.AspNetCore.WebUtilities;
using ApelMusicAPI.Email;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Net;

namespace ApelMusicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserData userData;
        private readonly IConfiguration _configuration;
        private readonly EmailService emailService;

        public UserController(UserData userData, IConfiguration configuration, EmailService emailService)
        {
            this.userData = userData;
            _configuration = configuration;
            this.emailService = emailService;
        }


        [HttpPost("CreateAccount")]
        public async Task<IActionResult> CreateAccount([FromBody] UserDTO userDTO)
        {
            try
            {
                if (userDTO.userEmail == userData.GetUserEmail(userDTO.userEmail)) return Problem("The email is already used");

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
                    bool sendEmailActivation = await SendEmailActivation(newUser);
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

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginRequestDTO credential)
        {
            if (credential is null)
                return BadRequest("Invalid client request");

            if (string.IsNullOrEmpty(credential.userEmail) || string.IsNullOrEmpty(credential.userPassword))
                return BadRequest("Invalid client request");

            User? user = userData.CheckUserAuth(credential.userEmail);

            if (user == null)
                return Unauthorized("You do not authorized");

            string userRole = userData.GetRoleNameById(user.role);

            bool isVerified = BCrypt.Net.BCrypt.Verify(credential.userPassword, user?.userPassword);
            //bool isVerified = user?.Password == credential.Password;

            bool isActivated = userData.GetUserActivationCheck(credential.userEmail);

            if ((user != null && !isVerified) || !isActivated)
            {
                return BadRequest("Incorrect Credential!");
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

                return Ok(new LoginResponseDTO { userId = user.userId, token = token, role = userRole });
            }
        }


        private async Task<bool> SendEmailActivation(User user)
        {
            if (user == null || string.IsNullOrEmpty(user.userEmail)) return false;

            List<string> emailTo = new List<string>();
            emailTo.Add(user.userEmail);

            string subject = "Account Activation";

            Dictionary<string, string?> param = new Dictionary<string, string?>
            {
                { "user_id", user.userId.ToString() },
                { "user_email", user.userEmail }
            };

            string callbackUrl = QueryHelpers.AddQueryString("https://localhost:7293/api/User/ActivateUser", param);
            EmailActivationModel emailActivationModel = new EmailActivationModel()
            {
                Email = user.userEmail,
                Link = callbackUrl
            };

            string body = emailService.GetEmailTemplate(emailActivationModel, MailConstant.EmailTemplate);


            EmailModel emailModel = new EmailModel(emailTo, subject, body);
            bool mailResult = await emailService.SendEmailAsync(emailModel, new CancellationToken());

            return mailResult;
        }


        [HttpGet("ActivateUser")]
        public IActionResult ActivateUser(Guid user_id, string user_email)
        {
            try
            {
                User? user = userData.CheckUserAuth(user_email);

                if (user == null)
                    return BadRequest("Activation Failed");

                if (user.isActivated == true)
                    return BadRequest("User has been activated");

                bool result = userData.ActivateUser(user_id);

                if (result)
                    return Redirect("http://52.237.194.35:2025/confirm");
                else
                    return StatusCode(500, "Activation Failed");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email)) return BadRequest("Email is empty");

                bool sendMail = await SendEmailForgetPassword(email);

                if (sendMail)
                {
                    return Ok("Mail sent");
                }
                else
                {
                    return StatusCode(500, "Error");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        private async Task<bool> SendEmailForgetPassword(string email)
        {
            // send email
            List<string> emailTo = new List<string>();
            emailTo.Add(email);

            string subject = "Forget Password";

            Dictionary<string, string?> param = new Dictionary<string, string?>
            {
                {"email", email }
            };

            string userEmail;
            string callbackUrl;
            if (param.TryGetValue("email", out userEmail))
            {
                callbackUrl = "http://52.237.194.35:2025/newPassword/" + userEmail;
            }
            else
            {
                callbackUrl = string.Empty;
            }
            EmailActivationModel emailActivationModel = new EmailActivationModel()
            {
                Email = email,
                Link = callbackUrl
            };
            string body = emailService.GetEmailTemplate(emailActivationModel, MailConstant.ForgotPasswordEmailTemplate);

            EmailModel emailModel = new EmailModel(emailTo, subject, body);
            bool mailResult = await emailService.SendEmailAsync(emailModel, new CancellationToken());

            return mailResult;
        }

        [HttpPost("ResetPassword")]
        public IActionResult ResetPassword([FromBody] ResetPasswordDTO resetPassword)
        {
            try
            {
                if (resetPassword == null) return BadRequest("No Data");

                if (resetPassword.Password != resetPassword.ConfirmPassword)
                {
                    return BadRequest("Password doesn't match");
                }

                bool reset = userData.ResetPassword(resetPassword.Email, BCrypt.Net.BCrypt.HashPassword(resetPassword.Password));

                if (reset)
                {
                    return Ok("Reset password OK");
                }
                else
                {
                    return StatusCode(500, "Error");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
