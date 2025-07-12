using System.Collections.Generic;

namespace POS_API.Utilities.Authentication
{
    public interface IAuthenticationUtilities
    {
        Dictionary<string, string> GetClaims(string token);
        string GetAccessToken(string Id, string UserName = "", string FirstName = "", string LastName = "", string CompanyId = "", string CompanyName = "", string RoleId = "", string RoleName = "");
    }
}
