using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;

namespace HelloGreetingApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelloGreetingController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() 
        {
            ResponseModel<string> responceModel = new ResponseModel<string>();
            responceModel.Success = true;
            responceModel.Message = "Greeting Successful";
            responceModel.Data = "Hello, World!";
            return Ok(responceModel);
        }

        [HttpPost]
        public void Post([FromBody] UserRegistrationModel userRegistrationModel) 
        {
            ResponseModel<string> responseModel = new ResponseModel<string>();
            responseModel.Success = true;
            responseModel.Message = "User Added successfully";
            responseModel.Data = $"First Name : {userRegistrationModel.FirstName} Last Name : {userRegistrationModel.LastName} Email : {userRegistrationModel.Email}";
        }

        [HttpPut]
        public IActionResult Put([FromBody] UserRegistrationModel userRegistrationModel)
        {
            ResponseModel<string> responseModel = new ResponseModel<string>();
            responseModel.Success = true;
            responseModel.Message = "User updated successfully";
            //UserRegistrationModel userRegistrationModel = UserRegistrationModel.GetUserByEmail().
            responseModel.Data = $"Updated User - First Name : {userRegistrationModel.FirstName} Last Name : {userRegistrationModel.LastName} Email : {userRegistrationModel.Email}";
            return Ok(responseModel);
        }

        [HttpPatch]
        public void Patch(UpdateRequestModel updateRequestModel) 
        {
            ResponseModel<string> responseModel = new ResponseModel<string>();
            responseModel.Success = true;
            responseModel.Message = "FirstName updated successfully";
            responseModel.Data = $"Updated User - First Name : {updateRequestModel.FirstName}";

        }

        [HttpDelete]
        public void Delete(UserRegistrationModel userDeletion) 

        {
            ResponseModel<string> responseModel = new ResponseModel<string>();
            responseModel.Success = true;
            responseModel.Message = "User Deleted successfully";
            responseModel.Data = $"User deleted successfully with email : {userDeletion.Email}";
        }
    }
}
