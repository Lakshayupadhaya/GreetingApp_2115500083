using BusinessLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using ModelLayer.DTO;
using ModelLayer.Model;

namespace HelloGreetingApplication.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class UserController : Controller
    {
        private readonly IUserBL _userAuthBL;
        public UserController(IUserBL userAuthBL)
        {
            _userAuthBL = userAuthBL;
        }
        [HttpPost]
        [Route("/register")]
        public ActionResult RegisterUser([FromBody] UserRegistrationDTO newUser)
        {
            Responce<RegisterResponceDTO> newUserResponce = _userAuthBL.RegisterUserBL(newUser);
            return Ok(newUserResponce);
        }

        [HttpPost]
        [Route("/login")]
        public ActionResult LoginUser(LoginDTO loginDetails)
        {
            Responce<string> responce = _userAuthBL.LoginUserBL(loginDetails);
            return Ok(responce);
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO forgotPasswordDTO)
        {
            var response = await _userAuthBL.ForgotPasswordBL(forgotPasswordDTO.Email);
            if (!response.found)
            {
                return NotFound(new { message = "Email id not found" });
            }
            if (!response.Sent)
                return BadRequest(new { message = "Failed to send reset email" });

            return Ok(new { message = "Password reset email sent successfully" });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPasswordDTO)
        {
            var response = await _userAuthBL.ResetPasswordBL(resetPasswordDTO.Token, resetPasswordDTO.NewPassword);
            if (!response)
                return BadRequest(new { message = "Invalid or expired reset token" });

            return Ok(new { message = "Password reset successful" });
        }
    }
}
