using System;
using System.Collections.Generic;
using System.Linq;
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

        public GreetingRL(GreetingAppContext dbContext, ILogger<GreetingRL> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

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
            _logger.LogInformation("Starting the process of saving the greeting to database");
            _dbContext.Greetings.Add(greetingEntity);
            _dbContext.SaveChanges();
            _logger.LogInformation("Greeting saved to database successfully");

            return new GreetingEntity { Id = greetingEntity.Id, Greeting = greetingEntity.Greeting };
        }

        public (string greeting, bool condition) GetGreetingByIdRL(int id)
        {
            var greeting = _dbContext.Greetings.FirstOrDefault(g => g.Id == id);
            return greeting != null ? (greeting.Greeting, true) : ($"Greeting not found for Id: {id}", false);
        }

        public List<string> GetGreetingsRL()
        {
            _logger.LogInformation("Fetching greetings from dataset");
            return _dbContext.Greetings.Select(g => g.Greeting).ToList();
        }

        public (bool condition, string status, string greeting) EditGreetingRL(GreetingEntity editGreetingRequest)
        {
            var greeting = _dbContext.Greetings.FirstOrDefault(i => i.Id == editGreetingRequest.Id);
            if (greeting == null)
            {
                return (false, "Greeting Not Found", editGreetingRequest.Greeting);
            }

            greeting.Greeting = editGreetingRequest.Greeting;
            _dbContext.SaveChanges();
            return (true, "Greeting Edited", greeting.Greeting);
        }

        public (bool condition, string status, string greeting) DeleteGreetingRL(GreetingEntity editGreetingRequest)
        {
            var greeting = _dbContext.Greetings.FirstOrDefault(i => i.Id == editGreetingRequest.Id);
            if (greeting == null)
            {
                return (false, "Greeting Not Found", $"No greeting found for ID: {editGreetingRequest.Id}");
            }

            string deletedGreeting = greeting.Greeting;
            _dbContext.Remove(greeting);
            _dbContext.SaveChanges();
            return (true, "Greeting Removed", deletedGreeting);
        }
    }
}