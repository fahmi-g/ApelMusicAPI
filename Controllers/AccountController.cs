using ApelMusicAPI.Data;
using ApelMusicAPI.DTOs;
using ApelMusicAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApelMusicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountData accountData;
        private readonly UserData userData;

        public AccountController(AccountData accountData, UserData userData)
        {
            this.accountData = accountData;
            this.userData = userData;
        }

        [HttpGet("GetAccounts")]
        public IActionResult GetClasses()
        {
            try
            {
                List<User> users = accountData.GetAllUser();
                return StatusCode(200, users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetAccount")]
        public IActionResult GetAccountById(Guid user_id)
        {
            try
            {
                User user = accountData.GetUserById(user_id);
                return StatusCode(200, user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("AddAccount")]
        public IActionResult AddAccount([FromBody] AddAccountDTO account)
        {
            try
            {
                if (account.userEmail == userData.GetUserEmail(account.userEmail)) return Problem("The email is already used");

                User newAccount = new User
                {
                    userId = Guid.NewGuid(),
                    userName = account.userName,
                    userEmail = account.userEmail,
                    userPassword = BCrypt.Net.BCrypt.HashPassword(account.userPassword),
                    role = account.role,
                    isActivated = account.isActivated
                };

                bool result = accountData.InsertAccount(newAccount);

                if (result)
                {
                    return StatusCode(201, $"{account.userName}, your account has been added.");
                }
                else
                {
                    return StatusCode(500, "Account addison failed.");
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPut("UpdateAccount")]
        public IActionResult UpdateCategory(Guid user_id, [FromBody] EditAccountDTO editAccountDTO)
        {
            if (editAccountDTO == null) return BadRequest("Data should be inputed");

            User newAccount = new User
            {
                userName = editAccountDTO.userName,
                userEmail = editAccountDTO.userEmail,
                role = editAccountDTO.role,
                isActivated = editAccountDTO.isActivated
            };

            bool result = accountData.EditAccount(user_id, newAccount);

            if (result) return StatusCode(200, "Account is successfully updated");
            else return StatusCode(500, "Error Occur");
        }
    }
}
