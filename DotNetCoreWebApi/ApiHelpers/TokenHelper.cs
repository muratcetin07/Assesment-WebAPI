using Microsoft.IdentityModel.Tokens;
using Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCoreWebApi.ApiHelpers
{
    public class TokenHelper
    {
        public static string Authenticate(User user)
        {
            var userToken = string.Empty;

            if (user == null)
                return null;


            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(AppConfiguration.Configuration.Secret);
            var signKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.EmailAddress),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                }),

                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(signKey, SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            userToken = tokenHandler.WriteToken(token);

            return userToken;
        }
    }
}
