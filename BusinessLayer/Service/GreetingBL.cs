using System;
using System.Collections.Generic;
using BusinessLayer.Interface;
using ModelLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Helper;
using RepositoryLayer.Interface;

namespace BusinessLayer.Service
{
    public class GreetingBL : IGreetingBL
    {
        private readonly IGreetingRL _greetingRL;
        private readonly Jwt _jwt;
        private readonly IRedisCacheService _cacheService;

        public GreetingBL(IGreetingRL greetingRL, Jwt jwt, IRedisCacheService cacheService)
        {
            _greetingRL = greetingRL;
            _jwt = jwt;
            _cacheService = cacheService;
        }

        public string GetGreetingBL()
        {
            string cacheKey = "GreetingMessage";
            var cachedGreeting = _cacheService.GetCache<string>(cacheKey);

            if (cachedGreeting != null)
            {
                return cachedGreeting;
            }

            string greeting = _greetingRL.GetGreetingRL();

            if (!string.IsNullOrEmpty(greeting))
            {
                _cacheService.SetCache(cacheKey, greeting, TimeSpan.FromMinutes(30));
            }

            return greeting;
        }

        public string GetGreetingBL(GreetingRequestModel greetingRequest)
        {
            if (!string.IsNullOrEmpty(greetingRequest.FirstName) && !string.IsNullOrEmpty(greetingRequest.LastName))
            {
                return $"Hello {greetingRequest.FirstName} {greetingRequest.LastName}";
            }
            else if (!string.IsNullOrEmpty(greetingRequest.FirstName))
            {
                return $"Hello {greetingRequest.FirstName}";
            }
            else if (!string.IsNullOrEmpty(greetingRequest.LastName))
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
            GreetingEntity greetingEntity = new GreetingEntity { Greeting = greetingMsg, UserId = userId };
            GreetingEntity greetingEntityResponce = _greetingRL.SaveGreetingRL(greetingEntity);

            // Invalidate cached greetings list
            _cacheService.RemoveCache($"Greetings_{userId}");

            return (true, greetingEntityResponce);
        }

        public (string greeting, bool condition) GetGreetingByIdBL(int id)
        {
            string cacheKey = $"Greeting_{id}";
            var cachedGreeting = _cacheService.GetCache<(string, bool)>(cacheKey);

            if (cachedGreeting != default)
            {
                return cachedGreeting;
            }

            var result = _greetingRL.GetGreetingByIdRL(id);
            _cacheService.SetCache(cacheKey, result, TimeSpan.FromMinutes(30));

            return result;
        }

        public (bool authorised, bool found, GreetingsModel) GetGreetingsBL(string token)
        {
            var result = _jwt.GetUserIdFromToken(token);
            if (result == null)
            {
                return (false, false, new GreetingsModel());
            }

            string cacheKey = $"Greetings_{result.Value}";
            var cachedList = _cacheService.GetCache<List<string>>(cacheKey);

            if (cachedList != null)
            {
                return (true, true, new GreetingsModel { Greetings = cachedList });
            }

            List<string> list = _greetingRL.GetGreetingsRL(result.Value);
            if (list == null)
            {
                return (true, false, new GreetingsModel());
            }

            _cacheService.SetCache(cacheKey, list, TimeSpan.FromMinutes(30));

            return (true, true, new GreetingsModel { Greetings = list });
        }

        public (bool condition, string status, string greeting) EditGreetingBL(IdRequestModel editGreetingRequest)
        {
            GreetingEntity editGreeting = new GreetingEntity { Id = editGreetingRequest.Id, Greeting = editGreetingRequest.Greeting };

            var result = _greetingRL.EditGreetingRL(editGreeting);

            // Invalidate cache for the updated greeting
            _cacheService.RemoveCache($"Greeting_{editGreetingRequest.Id}");

            return result;
        }

        public (bool condition, string status, string greeting) DeleteGreetingBL(int id)
        {
            var result = _greetingRL.DeleteGreetingRL(id);

            // Remove deleted greeting from cache
            _cacheService.RemoveCache($"Greeting_{id}");

            return result;
        }
    }
}
