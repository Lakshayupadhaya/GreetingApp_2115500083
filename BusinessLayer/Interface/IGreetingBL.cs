using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;

namespace BusinessLayer.Interface
{
    public interface IGreetingBL
    {
        string GetGreetingBL();

        string GetGreetingBL(GreetingRequestModel greetingRequest);

        ResponseModel<string> SaveGreetingBL(GreetingRequestModel saveGreetingRequest);

        (string greeting, bool condition) GetGreetingByIdBL(int id);

        GreetingsModel GetGreetingsBL();

        (bool condition, string status, string greeting) EditGreetingBL(IdRequestModel editGreetingRequest);

        (bool condition, string status, string greeting) DeleteGreetingBL(DeleteRequestModel deleteGreetingRequest);
    }
}
