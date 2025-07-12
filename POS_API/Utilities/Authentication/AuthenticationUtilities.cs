using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace POS_API.Utilities.Authentication
{
    public class AuthenticationUtilities:IAuthenticationUtilities
    {
        private readonly IConfiguration _config;

        public AuthenticationUtilities(IConfiguration config) => _config = config;


        public string GetAccessToken(string Id, string UserName = "", string FirstName = "", string LastName = "", string CompanyId = "", string CompanyName = "", string RoleId = "", string RoleName = "")
        {
            var claims = new[]
                {
                    new Claim(CustomClaimTypes.Id, Id),
                    new Claim(CustomClaimTypes.UserName, UserName),
                    new Claim(CustomClaimTypes.FirstName, FirstName),
                    new Claim(CustomClaimTypes.LastName, LastName),
                    new Claim(CustomClaimTypes.CompanyId, CompanyId),
                    new Claim(CustomClaimTypes.CompanyName, CompanyName),
                    new Claim(CustomClaimTypes.RoleId, RoleId),
                    new Claim(CustomClaimTypes.RoleName, RoleName),
                };
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var strToken = new { token = tokenHandler.WriteToken(token: token) };
            return strToken.token;
        }
        public Dictionary<string,string> GetClaims(string token) 
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var Id = tokenS.Claims.First(claim => claim.Type == CustomClaimTypes.Id);
            var UserName = tokenS.Claims.First(claim => claim.Type == CustomClaimTypes.UserName);
            var FirstName = tokenS.Claims.First(claim => claim.Type == CustomClaimTypes.FirstName);
            var LastName = tokenS.Claims.First(claim => claim.Type == CustomClaimTypes.LastName);
            var CompanyId = tokenS.Claims.First(claim => claim.Type == CustomClaimTypes.CompanyId);
            var CompanyName = tokenS.Claims.First(claim => claim.Type == CustomClaimTypes.CompanyName);
            var RoleId = tokenS.Claims.First(claim => claim.Type == CustomClaimTypes.RoleId);
            var RoleName = tokenS.Claims.First(claim => claim.Type == CustomClaimTypes.RoleName);
            var claims = new Dictionary<string, string>() 
            {
                {Id.Type, Id.Value},
                {UserName.Type, UserName.Value},
                {FirstName.Type, FirstName.Value},
                {LastName.Type, LastName.Value},
                {CompanyId.Type, CompanyId.Value},
                {CompanyName.Type, CompanyName.Value},
                {RoleId.Type, RoleId.Value},
                {RoleName.Type, RoleName.Value},
            };
            return claims;
        }
    }
    public static class CustomClaimTypes
    {
        public const string Id = "Id";
        public const string UserName = "UserName";
        public const string FirstName = "FirstName";
        public const string LastName = "LastName";
        public const string CompanyId = "CompanyId";
        public const string BranchId = "BranchId";
        public const string CompanyName = "CompanyName";
        public const string RoleId = "RoleId";
        public const string RoleName = "RoleName";
    }
}
