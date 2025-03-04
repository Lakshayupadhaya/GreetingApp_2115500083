using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryLayer.Interface;

namespace RepositoryLayer.Service
{
    public class GreetingRL : IGreetingRL
    {
        private string GreetingMsg = "Hello World";

        //Method to get the greeting message
        private string GetMessage()
        {
            return GreetingMsg;
        }
        public string GetGreetingRL()
        {
            return GetMessage();
        }
    }
}
