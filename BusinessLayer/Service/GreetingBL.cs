using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using ModelLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Helper;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;

namespace BusinessLayer.Service
{
    public class GreetingBL : IGreetingBL
    {
        private readonly IGreetingRL _greetingRL;
        private readonly Jwt _jwt;

        public GreetingBL(IGreetingRL greetingRL, Jwt jwt)
        {
            _greetingRL = greetingRL;
            _jwt = jwt;
        }

        //Method to get  greeting form repository layer
        public string GetGreetingBL()
        {
            return _greetingRL.GetGreetingRL();
        }

        public string GetGreetingBL(GreetingRequestModel greetingRequest) 
        {
            if ((!String.IsNullOrEmpty(greetingRequest.FirstName)) && (!String.IsNullOrEmpty(greetingRequest.LastName)))
            {
                return $"Hello {greetingRequest.FirstName} {greetingRequest.LastName}";
            }

            else if ((!String.IsNullOrEmpty(greetingRequest.FirstName))) 
            {
                return $"Hello {greetingRequest.FirstName}";
            }
            else if ((!String.IsNullOrEmpty(greetingRequest.LastName))) 
            {
                return $"Hello {greetingRequest.LastName}";
            }
            else 
            {
                return "Hello World";
            }
        }

        public (bool authorised, GreetingEntity) SaveGreetingBL(GreetingRequestModel saveGreetingRequest, string token)
        {

            string greetingMsg = GetGreetingBL(saveGreetingRequest);
            var result = _jwt.GetUserIdFromToken(token);
            if (result == null) 
            {
                return (false, new GreetingEntity());
            }
            int userId = result.Value;
            GreetingEntity greetingEntity = new GreetingEntity();
            greetingEntity.Greeting = greetingMsg;
            greetingEntity.UserId = userId;

            GreetingEntity greetingEntityResponce = _greetingRL.SaveGreetingRL(greetingEntity);

            return (true, greetingEntityResponce);

            //ResponseModel<string> responce = new ResponseModel<string>();

            //responce.Success = true;
            //responce.Message = "Process successfull";
            //responce.Data = greetingEntityResponce.ToString();

            //return responce;
        }

        public (string greeting, bool condition) GetGreetingByIdBL(int id)
        {
           (string greeting, bool condition) = _greetingRL.GetGreetingByIdRL(id);

            return (greeting, condition);

            
        }


        public (bool authorised, bool found, GreetingsModel) GetGreetingsBL(string token) 
        {
            var result = _jwt.GetUserIdFromToken(token);
            if(result == null) 
            {
                return (false, false, new GreetingsModel());
            }
            List<string> list = _greetingRL.GetGreetingsRL(result.Value);
            if(list == null) 
            {
                return (true, false, new GreetingsModel());
            }

            return (true, true, new GreetingsModel { Greetings = list });
        }

        public  (bool condition, string status, string greeting) EditGreetingBL(IdRequestModel editGreetingRequest)
        {
            GreetingEntity editGreeting = new GreetingEntity();
            editGreeting.Id = editGreetingRequest.Id;
            editGreeting.Greeting = editGreetingRequest.Greeting;
            return _greetingRL.EditGreetingRL(editGreeting);
        }

        public (bool condition, string status, string greeting) DeleteGreetingBL(int id)
        {
            //GreetingEntity deleteGreeting = new GreetingEntity();
            //deleteGreeting.Id = id;
            return _greetingRL.DeleteGreetingRL(id);
        }


    }
}
