using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;

namespace RepositoryLayer.Service
{
    public class GreetingRL : IGreetingRL
    {
        private string GreetingMsg = "Hello World";

        private readonly GreetingAppContext _dbContext;
        private readonly ILogger<GreetingRL> _logger;

        public GreetingRL (GreetingAppContext dbContext, ILogger<GreetingRL> logger) 
        {
            _dbContext = dbContext;
            _logger = logger;
        }    

        //Method to get the greeting message
        private string GetMessage()
        {
            return GreetingMsg;
        }
        public string GetGreetingRL()
        {
            return GetMessage();
        }


        public GreetingEntity SaveGreetingRL(GreetingEntity greetingEntity)  
        {
            try 
            {
                _logger.LogInformation("Starting the process of Saving the greeting to database");
                _dbContext.Greetings.Add(greetingEntity);
                _dbContext.SaveChanges();//key ID is saved and is reflected to the object so we can access it 
                _logger.LogInformation("Greeting saved to database successfully");
                int id = greetingEntity.Id;
                GreetingEntity greetingResponce = new GreetingEntity();
                greetingResponce.Id = id;
                greetingResponce.Greeting = greetingEntity.Greeting;

                return greetingResponce;
            }
            catch (Exception ex) 
            {
                _logger.LogError($"Error occured while saving the greeting to database database : {ex.Message}");
                GreetingEntity greetingResponce = new GreetingEntity();

                greetingResponce.Greeting = ex.Message;
                greetingResponce.Id = -1;
                return greetingResponce;
            }
            
            
            

        }

        public (string greeting, bool condition) GetGreetingByIdRL(int id) 
        {
            var greeting = _dbContext.Greetings.FirstOrDefault(greeting => greeting.Id == id);

            if( greeting == null)
            {
                return ($"Greeting not found for Id : {id}", false);
            }
            return (greeting.Greeting, true);
        }
    }
}
