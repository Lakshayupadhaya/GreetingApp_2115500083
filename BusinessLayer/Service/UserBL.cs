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
using BusinessLayer.Email;

namespace BusinessLayer.Service
{
    public class UserBL : IUserBL
    {
        private readonly IUserRL _userAuthRL;
        private readonly Jwt _jwt;
        private readonly EmailHelper _email;
        public UserBL(IUserRL userAuthRL, Jwt jwt, EmailHelper email)
        {
            _userAuthRL = userAuthRL;
            _jwt = jwt;
            _email = email;
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
        public async Task<(bool Sent, bool found)> ForgotPasswordBL(string email)
        {
            bool exists = _userAuthRL.Checkuser(email);
            if (!exists)
            {
                return (false, false);
            }

            var resetToken = _jwt.GenerateResetToken(email);



            // Publish message to RabbitMQ
            //var message = new { Email = email, ResetToken = resetToken };
            //_rabitMQProducer.PublishMessage(message);

            await _email.SendPasswordResetEmailAsync(email, resetToken);


            return (true, true); // Assume success (actual sending happens in Consumer)
        }

        public async Task<bool> ResetPasswordBL(string token, string newPassword)
        {
            var email = _jwt.ValidateResetToken(token);
            if (email == null) return false;

            string newHashPassword = PasswordHasher.HashPassword(newPassword);

            return await _userAuthRL.UpdateUserPassword(email, newHashPassword);
        }
    }
}
