using BusinessLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModelLayer;
using ModelLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Helper;

namespace HelloGreetingApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelloGreetingController : ControllerBase
    {
        private readonly IGreetingBL _greetingBL;
        private readonly ILogger<HelloGreetingController> _logger;
        private readonly Jwt _jwt;

        public HelloGreetingController(IGreetingBL greetingBL, ILogger<HelloGreetingController> logger, Jwt jwt)
        {
            _greetingBL = greetingBL;
            _logger = logger;
            _jwt = jwt;
        }

        //[HttpGet]
        //public IActionResult Get()
        //{
        //    _logger.LogInformation("Starting process of getting greeting");
        //    string greetingMsg = _greetingBL.GetGreetingBL();
        //    _logger.LogInformation("Greeting successful");
        //    return Ok(new ResponseModel<string>
        //    {
        //        Success = true,
        //        Message = "Greeting Successful",
        //        Data = greetingMsg
        //    });
        //}

        //[HttpPost]
        //public IActionResult Post([FromBody] UserRegistrationModel userRegistrationModel)
        //{
        //    _logger.LogInformation("Starting process of registering user");
        //    return Ok(new ResponseModel<string>
        //    {
        //        Success = true,
        //        Message = "User added successfully",
        //        Data = $"First Name: {userRegistrationModel.FirstName}, Last Name: {userRegistrationModel.LastName}, Email: {userRegistrationModel.Email}"
        //    });
        //}

        //[HttpPut]
        //public IActionResult Put([FromBody] UserRegistrationModel userRegistrationModel)
        //{
        //    _logger.LogInformation($"Updating user with Email: {userRegistrationModel.Email}");
        //    return Ok(new ResponseModel<string>
        //    {
        //        Success = true,
        //        Message = "User updated successfully",
        //        Data = $"Updated User - First Name: {userRegistrationModel.FirstName}, Last Name: {userRegistrationModel.LastName}, Email: {userRegistrationModel.Email}"
        //    });
        //}

        //[HttpPatch]
        //public IActionResult Patch(UpdateRequestModel updateRequestModel)
        //{
        //    _logger.LogInformation("Updating value for user");
        //    return Ok(new ResponseModel<string>
        //    {
        //        Success = true,
        //        Message = "First Name updated successfully",
        //        Data = $"Updated User - First Name: {updateRequestModel.FirstName}"
        //    });
        //}

        //[HttpDelete]
        //public IActionResult Delete(UserRegistrationModel userDeletion)
        //{
        //    _logger.LogInformation("Deleting user");
        //    return Ok(new ResponseModel<string>
        //    {
        //        Success = true,
        //        Message = "User deleted successfully",
        //        Data = $"User deleted successfully with email: {userDeletion.Email}"
        //    });
        //}

        [HttpPost]
        [Route("GetGreeting")]
        public IActionResult GetGreeting([FromBody] GreetingRequestModel greetingRequest)
        {
            _logger.LogInformation("Getting personalized greeting");
            string greetingMsg = _greetingBL.GetGreetingBL(greetingRequest);
            return Ok(new ResponseModel<string>
            {
                Success = true,
                Message = "Greeting Successful",
                Data = greetingMsg
            });
        }

        [HttpPost]
        [Route("SaveGreeting")]
        public IActionResult SaveGreeting(GreetingRequestModel saveGreetingRequest, [FromQuery] string token)
        {
            _logger.LogInformation("Saving greeting");
            (bool authorised, GreetingEntity savedGreeting) = _greetingBL.SaveGreetingBL(saveGreetingRequest, token);
            if(authorised) 
            {
                ResponseModel<GreetingEntity> responce = new ResponseModel<GreetingEntity>();

                responce.Success = true;
                responce.Message = "Process successfull";
                responce.Data = savedGreeting;

                return Ok(responce);
            }
            return Unauthorized((new ResponseModel<string>
            {
                Success = false,
                Message = "You are unauthorised"
                //Data = greeting
            }));
        }

        [HttpPost]
        [Route("GetGreeting/Id")]
        public IActionResult GetGreetingById([FromQuery] string token, int id)
        {
            bool authrised = _jwt.ValidateToken(token, id);
            if (authrised) 
            {
                (string greeting, bool condition) = _greetingBL.GetGreetingByIdBL(id);
                return Ok(new ResponseModel<string>
                {
                    Success = condition,
                    Message = condition ? "Greeting received successfully" : "Greeting not found",
                    Data = greeting
                });
            }
            return Unauthorized((new ResponseModel<string>
            {
                Success = false,
                Message = "You are unauthorised"
                //Data = greeting
            }));
        }

        [HttpGet]
        [Route("Greetings/All")]
        public IActionResult GetGreetings([FromQuery] string token)
        {

            _logger.LogInformation("Fetching all greetings");
            (bool authorised, bool found, GreetingsModel allGreetings) = _greetingBL.GetGreetingsBL(token);
            if (authorised) 
            {
                if (found) 
                {
                    Responce<GreetingsModel> responce = new Responce<GreetingsModel>();
                    responce.Success = true;
                    responce.Message = "Greetings fetched successfully";
                    responce.Data = allGreetings;
                    return Ok(responce);
                }
                Responce<GreetingsModel> notFoundresponce = new Responce<GreetingsModel>();
                notFoundresponce.Success = false;
                notFoundresponce.Message = "No greetings";
                notFoundresponce.Data = allGreetings;
                return Ok(notFoundresponce);
            }
            return Unauthorized(new ResponseModel<string>
            {
                Success = false,
                Message = "You are not authorised",
                //Data = allGreetings
            });
        }

        [HttpPatch]
        [Route("Greeting/Edit")]
        public IActionResult EditGreeting([FromBody] IdRequestModel editGreetingRequest,[FromQuery] string token)
        {
            bool authorised = _jwt.ValidateToken(token, editGreetingRequest.Id);
            if (authorised) 
            {
                (bool condition, string status, string greeting) = _greetingBL.EditGreetingBL(editGreetingRequest);
                return condition ? Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = status,
                    Data = "Edited Greeting: " + greeting
                }) : BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = status,
                    Data = greeting
                });
            }
            return Unauthorized((new ResponseModel<string>
            {
                Success = false,
                Message = "You are unauthorised"
                //Data = greeting
            }));
        }

        [HttpDelete]
        [Route("Greeting/Delete")]
        public IActionResult DeleteGreeting( int id, [FromQuery] string token)
        {
            bool authorised = _jwt.ValidateToken(token, id);
            if (authorised) 
            {
                (bool condition, string status, string greeting) = _greetingBL.DeleteGreetingBL(id);
                return condition ? Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = status,
                    Data = "Deleted Greeting: " + greeting
                }) : BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = status,
                    Data = greeting
                });
            }
            return Unauthorized((new ResponseModel<string>
            {
                Success = false,
                Message = "You are unauthorised"
                //Data = greeting
            }));
        }
    }
}
