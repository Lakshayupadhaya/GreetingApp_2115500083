using BusinessLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModelLayer.Model;

namespace HelloGreetingApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelloGreetingController : ControllerBase
    {
        private readonly IGreetingBL _greetingBL;
        private readonly ILogger<HelloGreetingController> _logger;

        public HelloGreetingController(IGreetingBL greetingBL, ILogger<HelloGreetingController> logger)
        {
            _greetingBL = greetingBL;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("Starting process of getting greeting");
            string greetingMsg = _greetingBL.GetGreetingBL();
            _logger.LogInformation("Greeting successful");
            return Ok(new ResponseModel<string>
            {
                Success = true,
                Message = "Greeting Successful",
                Data = greetingMsg
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody] UserRegistrationModel userRegistrationModel)
        {
            _logger.LogInformation("Starting process of registering user");
            return Ok(new ResponseModel<string>
            {
                Success = true,
                Message = "User added successfully",
                Data = $"First Name: {userRegistrationModel.FirstName}, Last Name: {userRegistrationModel.LastName}, Email: {userRegistrationModel.Email}"
            });
        }

        [HttpPut]
        public IActionResult Put([FromBody] UserRegistrationModel userRegistrationModel)
        {
            _logger.LogInformation($"Updating user with Email: {userRegistrationModel.Email}");
            return Ok(new ResponseModel<string>
            {
                Success = true,
                Message = "User updated successfully",
                Data = $"Updated User - First Name: {userRegistrationModel.FirstName}, Last Name: {userRegistrationModel.LastName}, Email: {userRegistrationModel.Email}"
            });
        }

        [HttpPatch]
        public IActionResult Patch(UpdateRequestModel updateRequestModel)
        {
            _logger.LogInformation("Updating value for user");
            return Ok(new ResponseModel<string>
            {
                Success = true,
                Message = "First Name updated successfully",
                Data = $"Updated User - First Name: {updateRequestModel.FirstName}"
            });
        }

        [HttpDelete]
        public IActionResult Delete(UserRegistrationModel userDeletion)
        {
            _logger.LogInformation("Deleting user");
            return Ok(new ResponseModel<string>
            {
                Success = true,
                Message = "User deleted successfully",
                Data = $"User deleted successfully with email: {userDeletion.Email}"
            });
        }

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
        public IActionResult SaveGreeting(GreetingRequestModel saveGreetingRequest)
        {
            _logger.LogInformation("Saving greeting");
            return Ok(_greetingBL.SaveGreetingBL(saveGreetingRequest));
        }

        [HttpPost]
        [Route("GetGreeting/Id")]
        public IActionResult GetGreetingById([FromBody] IdRequestModel greetingRequestId)
        {
            (string greeting, bool condition) = _greetingBL.GetGreetingByIdBL(greetingRequestId.Id);
            return Ok(new ResponseModel<string>
            {
                Success = condition,
                Message = condition ? "Greeting received successfully" : "Greeting not found",
                Data = greeting
            });
        }

        [HttpGet]
        [Route("Greetings/All")]
        public IActionResult GetGreetings()
        {
            _logger.LogInformation("Fetching all greetings");
            GreetingsModel allGreetings = _greetingBL.GetGreetingsBL();
            return Ok(new ResponseModel<GreetingsModel>
            {
                Success = true,
                Message = "Greetings fetched successfully",
                Data = allGreetings
            });
        }

        [HttpPatch]
        [Route("Greeting/Edit")]
        public IActionResult EditGreeting([FromBody] IdRequestModel editGreetingRequest)
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

        [HttpDelete]
        [Route("Greeting/Delete")]
        public IActionResult DeleteGreeting(DeleteRequestModel deleteRequest)
        {
            (bool condition, string status, string greeting) = _greetingBL.DeleteGreetingBL(deleteRequest);
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
    }
}
