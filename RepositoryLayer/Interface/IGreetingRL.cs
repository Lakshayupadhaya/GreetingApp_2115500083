using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interface
{
    public interface IGreetingRL
    {
        string GetGreetingRL();

        GreetingEntity SaveGreetingRL(GreetingEntity greetingEntity);

        (string greeting, bool condition) GetGreetingByIdRL(int id);

        public List<string> GetGreetingsRL(int userId);

        (bool condition, string status, string greeting) EditGreetingRL(GreetingEntity editGreetingRequest);

        (bool condition, string status, string greeting) DeleteGreetingRL(int id);

        public bool CheckAction(int userId, int id);
    }
}
