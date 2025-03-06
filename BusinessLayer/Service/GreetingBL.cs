using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using ModelLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;

namespace BusinessLayer.Service
{
    public class GreetingBL : IGreetingBL
    {
        private readonly IGreetingRL _greetingRL;

        public GreetingBL(IGreetingRL greetingRL)
        {
            _greetingRL = greetingRL;
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

        public ResponseModel<string> SaveGreetingBL(GreetingRequestModel saveGreetingRequest)
        {
            string greetingMsg = GetGreetingBL(saveGreetingRequest);

            GreetingEntity greetingEntity = new GreetingEntity();
            greetingEntity.Greeting = greetingMsg;

            GreetingEntity greetingEntityResponce = _greetingRL.SaveGreetingRL(greetingEntity);

            ResponseModel<string> responce = new ResponseModel<string>();

            responce.Success = true;
            responce.Message = "Process successfull";
            responce.Data = greetingEntityResponce.ToString();

            return responce;
        }

        public (string greeting, bool condition) GetGreetingByIdBL(int id)
        {
           (string greeting, bool condition) = _greetingRL.GetGreetingByIdRL(id);

            return (greeting, condition);

            
        }


        public GreetingsModel GetGreetingsBL() 
        {
            List<string> list = _greetingRL.GetGreetingsRL();

            return new GreetingsModel { Greetings = list };
        }

        public  (bool condition, string status, string greeting) EditGreetingBL(IdRequestModel editGreetingRequest)
        {
            GreetingEntity editGreeting = new GreetingEntity();
            editGreeting.Id = editGreetingRequest.Id;
            editGreeting.Greeting = editGreetingRequest.Greeting;
            return _greetingRL.EditGreetingRL(editGreeting);
        }


    }
}
