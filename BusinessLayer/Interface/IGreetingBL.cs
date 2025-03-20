using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;
using RepositoryLayer.Entity;

namespace BusinessLayer.Interface
{
    public interface IGreetingBL
    {
        string GetGreetingBL();

        string GetGreetingBL(GreetingRequestModel greetingRequest);

        public (bool authorised, GreetingEntity) SaveGreetingBL(GreetingRequestModel saveGreetingRequest, string token);

        (string greeting, bool condition) GetGreetingByIdBL(int id);

        public (bool authorised, bool found, GreetingsModel) GetGreetingsBL(string token);

        (bool condition, string status, string greeting) EditGreetingBL(IdRequestModel editGreetingRequest);

        (bool condition, string status, string greeting) DeleteGreetingBL(int id);
    }
}
