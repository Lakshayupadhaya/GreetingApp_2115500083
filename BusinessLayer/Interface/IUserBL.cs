using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.DTO;
using ModelLayer;

namespace BusinessLayer.Interface
{
    public interface IUserBL
    {
        Responce<RegisterResponceDTO> RegisterUserBL(UserRegistrationDTO newUser);

        Responce<string> LoginUserBL(LoginDTO loginCrediantials);
    }
}
