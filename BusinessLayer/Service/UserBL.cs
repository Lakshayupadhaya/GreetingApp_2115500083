using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using ModelLayer.DTO;
using ModelLayer;
using RepositoryLayer.Interface;
using RepositoryLayer.Helper;
using RepositoryLayer.Entity;

namespace BusinessLayer.Service
{
    public class UserBL : IUserBL
    {
        private readonly IUserRL _userAuthRL;
        public UserBL(IUserRL userAuthRL)
        {
            _userAuthRL = userAuthRL;
        }
        public Responce<RegisterResponceDTO> RegisterUserBL(UserRegistrationDTO newUser)
        {
            bool Existing = _userAuthRL.Checkuser(newUser.Email);
            if (!Existing)
            {
                string hashPass = PasswordHasher.HashPassword(newUser.Password);
                //newUser.Password = hashPass;

                UserEntity newUserEntity = new UserEntity();
                newUserEntity.Email = newUser.Email;
                newUserEntity.Password = hashPass;
                newUserEntity.FirstName = newUser.FirstName;
                newUserEntity.LastName = newUser.LastName;

                UserEntity registeredUser = _userAuthRL.RegisterUserRL(newUserEntity);
                RegisterResponceDTO registerResponce = new RegisterResponceDTO();
                registerResponce.Email = registeredUser.Email;
                registerResponce.UserId = registeredUser.UserId;
                registerResponce.LastName = registeredUser.LastName;
                registerResponce.FirstName = registeredUser.FirstName;

                Responce<RegisterResponceDTO> registerResponceBack = new Responce<RegisterResponceDTO>();
                registerResponceBack.Success = true;
                registerResponceBack.Message = "User Registered Successfully";
                registerResponceBack.Data = registerResponce;
                return registerResponceBack;

            }
            Responce<RegisterResponceDTO> registerResponceBack1 = new Responce<RegisterResponceDTO>();
            registerResponceBack1.Success = false;
            registerResponceBack1.Message = "User Already Exists";

            RegisterResponceDTO registerResponceFailed = new RegisterResponceDTO();
            registerResponceFailed.Email = newUser.Email;

            registerResponceBack1.Data = registerResponceFailed;
            return registerResponceBack1;
        }
        public Responce<string> LoginUserBL(LoginDTO loginCrediantials)
        {
            //(bool Found, string HashPass) = _userAuthRL.GetUserCredentialsRL(loginCrediantials.Email);

            (bool login, string token) = _userAuthRL.LoginUserRL(loginCrediantials.Email, loginCrediantials.Password);

            if (login)
            {

                Responce<string> responce = new Responce<string>();
                responce.Success = true;
                responce.Message = "Login Successfull";
                responce.Data = token;
                return responce;
            }
            Responce<string> responce1 = new Responce<string>();
            responce1.Success = false;
            responce1.Message = "Incorrect Email or Password";
            responce1.Data = "No Token Generated";
            return responce1;
        }
    }
}
