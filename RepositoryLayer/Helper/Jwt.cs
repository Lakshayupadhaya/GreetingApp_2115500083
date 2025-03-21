using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;

namespace RepositoryLayer.Helper
{
    public class Jwt
    {
        private readonly IConfiguration _config;
        private readonly IGreetingRL _greetingRL;
        public Jwt(IConfiguration config, IGreetingRL greetingRl)
        {
            _config = config;
            _greetingRL = greetingRl;
        }
        public string GenerateToken(UserEntity user) //After marking it as static you are unable to access the _config so used dependency injection and not static method
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("UserId", user.UserId.ToString()),
                new Claim("Firstname", user.FirstName),
                new Claim("Lastname", user.LastName),
                //new Claim("UserName", UserName),
                new Claim("Email", user.Email)
                //new Claim("Role", user.Role)
                //new Claim("Phone", Phone)
                //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Unique token ID
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:ExpirationMinutes"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool ValidateToken(string token, int id)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                //if (principal == null) 
                //{
                //    return false;
                //}
                var userIdClaim = principal.FindFirst("UserId");
                int userId = int.Parse(userIdClaim.Value);

                bool authorised = _greetingRL.CheckAction(userId, id);



                return authorised; // Token is valid and id matched for the action
            }
            catch
            {
                return false; // Token is invalid or expired
            }
        }
        public int? GetUserIdFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                //var roleClaim = principal.FindFirst("Role");
                //string role = roleClaim.Value;
                var userIdClaim = principal.FindFirst("UserId");
                int userId = int.Parse(userIdClaim.Value);

                return userId;
            }
            catch
            {
                return null; // Return null if token is invalid
            }
        }
        public string GenerateResetToken(string email)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:ResetKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email)
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string ValidateResetToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_config["Jwt:ResetKey"]);

                var validationParams = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true
                };

                var principal = tokenHandler.ValidateToken(token, validationParams, out _);
                var emailClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

                return emailClaim?.Value;
            }
            catch
            {
                return null;
            }
        }
    }
}
