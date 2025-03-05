using BusinessLayer.Interface;
using BusinessLayer.Service;
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


        /// <summary>
        /// Get method to get the greeting message
        /// </summary>
        /// <returns>"Hello World"</returns>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                _logger.LogInformation("Starting process of getting greeting");
                ResponseModel<string> responceModel = new ResponseModel<string>();
                string greetingMsg = _greetingBL.GetGreetingBL();
                responceModel.Success = true;
                responceModel.Message = "Greeting Successful";
                responceModel.Data = greetingMsg;
                _logger.LogInformation("Greeting successfull");
                return Ok(responceModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while getting greeting {ex.Message}");
                ResponseModel<string> responceModel = new ResponseModel<string>();
                responceModel.Success = false;
                responceModel.Message = "OOPS error occured";
                responceModel.Data = ex.Message;

                return BadRequest(responceModel);
            }


        }

        /// <summary>
        /// Post method to register user
        /// </summary>
        /// <param name="userRegistrationModel"></param>
        /// <returns>responce of registration</returns>
        [HttpPost]
        public IActionResult Post([FromBody] UserRegistrationModel userRegistrationModel)
        {
            try
            {
                _logger.LogInformation("Starting post process of registering user");
                ResponseModel<string> responseModel = new ResponseModel<string>();
                responseModel.Success = true;
                responseModel.Message = "User Added successfully";
                responseModel.Data = $"First Name : {userRegistrationModel.FirstName} Last Name : {userRegistrationModel.LastName} Email : {userRegistrationModel.Email}";
                _logger.LogInformation($"User added successfully Email : {userRegistrationModel.Email}");
                return Ok(responseModel);
            }
            catch (Exception ex)
            {

                ResponseModel<string> responseModel = new ResponseModel<string>();
                responseModel.Success = false;
                responseModel.Message = "User registration process failed";
                responseModel.Data = ex.Message;
                _logger.LogError($"Error occured while registering user Error : {ex.Message}");
                return BadRequest(responseModel);
            }

        }
        /// <summary>
        /// Put method to update user
        /// </summary>
        /// <param name="userRegistrationModel"></param>
        /// <returns>Respone for user updation</returns>
        [HttpPut]
        public IActionResult Put([FromBody] UserRegistrationModel userRegistrationModel)
        {
            try
            {
                _logger.LogInformation($"Starting updating process for User with Email : {userRegistrationModel.Email}");
                ResponseModel<string> responseModel = new ResponseModel<string>();
                responseModel.Success = true;
                responseModel.Message = "User updated successfully";
                //UserRegistrationModel userRegistrationModel = UserRegistrationModel.GetUserByEmail().
                responseModel.Data = $"Updated User - First Name : {userRegistrationModel.FirstName} Last Name : {userRegistrationModel.LastName} Email : {userRegistrationModel.Email}";
                _logger.LogInformation($"User Updated succesfully new user with email : {userRegistrationModel.Email}");
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                ResponseModel<string> responseModel = new ResponseModel<string>();
                responseModel.Success = false;
                responseModel.Message = "error occured while updating user";
                responseModel.Data = ex.Message;
                _logger.LogError($"Error occured while updating user with Email : {userRegistrationModel.Email}");
                return BadRequest(responseModel);
            }

        }
        /// <summary>
        /// Patch mmethod to update a value
        /// </summary>
        /// <param name="updateRequestModel"></param>
        /// <returns>Responce of updated value</returns>
        [HttpPatch]
        public IActionResult Patch(UpdateRequestModel updateRequestModel)
        {
            try
            {
                _logger.LogInformation("Starting process of updatin value for user");
                ResponseModel<string> responseModel = new ResponseModel<string>();
                responseModel.Success = true;
                responseModel.Message = "FirstName updated successfully";
                responseModel.Data = $"Updated User - First Name : {updateRequestModel.FirstName}";
                _logger.LogInformation("value updated successfully");
                return Ok(responseModel);
            }
            catch (Exception ex)
            {

                ResponseModel<string> responseModel = new ResponseModel<string>();
                responseModel.Success = false;
                responseModel.Message = "error occured while updating firstname";
                responseModel.Data = ex.Message;
                _logger.LogError($"Error occured while updating value Error : {ex.Message}");
                return BadRequest(responseModel);
            }


        }
        /// <summary>
        /// Delete User
        /// </summary>
        /// <param name="userDeletion"></param>
        /// <returns>Deleted message</returns>
        [HttpDelete]
        public IActionResult Delete(UserRegistrationModel userDeletion)

        {
            try
            {
                _logger.LogInformation("Starting prcess of deleting user");
                ResponseModel<string> responseModel = new ResponseModel<string>();
                responseModel.Success = true;
                responseModel.Message = "User Deleted successfully";
                responseModel.Data = $"User deleted successfully with email : {userDeletion.Email}";
                userDeletion.FirstName = string.Empty;
                userDeletion.LastName = string.Empty;
                userDeletion.Email = string.Empty;
                userDeletion.Password = string.Empty;
                _logger.LogInformation("User deleted successfuly");
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                ResponseModel<string> responseModel = new ResponseModel<string>();
                responseModel.Success = false;
                responseModel.Message = "error occured while deleting user";
                responseModel.Data = ex.Message;
                _logger.LogError($"Error occured while deleting user Error : {ex.Message}");
                return BadRequest(responseModel);
            }

        }
        [HttpPost]
        [Route("GetGreeting")]
        public IActionResult GetGreeting([FromBody] GreetingRequestModel greetingRequest)
        {
            try
            {
                _logger.LogInformation("Starting process of getting greeting according to user");
                ResponseModel<string> responceModel = new ResponseModel<string>();
                string greetingMsg = _greetingBL.GetGreetingBL(greetingRequest);
                responceModel.Success = true;
                responceModel.Message = "Greeting Successful";
                responceModel.Data = greetingMsg;
                _logger.LogInformation("Greeting successfull");
                return Ok(responceModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while getting greeting {ex.Message}");
                ResponseModel<string> responceModel = new ResponseModel<string>();
                responceModel.Success = false;
                responceModel.Message = "OOPS error occured";
                responceModel.Data = ex.Message;

                return BadRequest(responceModel);
            }
        }

        [HttpPost]
        [Route("SaveGreeting")]
        public IActionResult SaveGreeting(GreetingRequestModel saveGreetingRequest)
        {
            try
            {
                _logger.LogInformation("Starting process of saving greeting according to user");
                ResponseModel<string> responceDB = _greetingBL.SaveGreetingBL(saveGreetingRequest);
                _logger.LogInformation("Greeting Saved successfully");
                return Ok(responceDB);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while saving greeting : {ex.Message}");
                ResponseModel<string> responce = new ResponseModel<string>();
                responce.Success = false;
                responce.Message = "Process failed";
                responce.Data = ex.Message;
                //ResponseModel<string> responceDB = _greetingBL.SaveGreetingBL(saveGreetingRequest);

                return BadRequest(responce);
            }
        }

        [HttpPost]
        [Route("GetGreeting/Id")]
        public IActionResult GetGreetingById([FromBody] IdRequestModel greetingRequestId)
        {
            try
            {
                (string greeting, bool condition) = _greetingBL.GetGreetingByIdBL(greetingRequestId.Id);
                if (condition)
                {
                    ResponseModel<string> responce1 = new ResponseModel<string>();
                    responce1.Success = condition;
                    responce1.Message = "Greeting Recieved succesfully";
                    responce1.Data = greeting;
                    return NotFound(responce1);
                }
                ResponseModel<string> responce = new ResponseModel<string>();
                responce.Success = condition;
                responce.Message = "Greeting Not found";
                responce.Data = greeting;
                return Ok(responce);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while getting greeting : {ex.Message}");
                ResponseModel<string> responce = new ResponseModel<string>();
                responce.Success = false;
                responce.Message = "Process failed";
                responce.Data = ex.Message;
                return BadRequest(responce);
            }
        }
    }
}
