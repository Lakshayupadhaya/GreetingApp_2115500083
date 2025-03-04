using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using ModelLayer.Model;
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


    }
}
