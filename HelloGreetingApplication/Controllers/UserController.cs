using BusinessLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using ModelLayer.DTO;

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
    }
}
