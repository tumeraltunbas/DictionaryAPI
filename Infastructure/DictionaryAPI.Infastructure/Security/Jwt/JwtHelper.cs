using DictionaryAPI.Application.Abstracts.Security.JWT;
using DictionaryAPI.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryAPI.Infastructure.Security.Jwt
{
    public class JwtHelper : IJwtHelper
    {
        IConfiguration _configuration;

        public JwtHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJwt(User user)
        {
            //Get symmetric of security key. From Microsoft.IdentityModel.Tokens
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Jwt:SecurityKey"))); //Get bytes of security key

            //Create SigningCredentials. From Microsoft.IdentityModel.Tokens
            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256); //Encryption of security key.

            //Claims
            Claim[] claims = new Claim[]
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email.ToString())
            };

            //JWT configuration. From System.IdentityModel.Tokens.Jwt
            JwtSecurityToken securityToken = new(
                audience: _configuration.GetValue<string>("Jwt:Audience"),
                issuer: _configuration.GetValue<string>("Jwt:Issuer"),
                expires: DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("Jwt:ExpiresInMinutes")),
                notBefore: DateTime.UtcNow,
                signingCredentials: signingCredentials,
                claims: claims
                );

            //Creating token
            JwtSecurityTokenHandler jwtHandler = new();
            return jwtHandler.WriteToken(securityToken);

        }
    }
}
