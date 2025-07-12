using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using POS_API.Utilities.Authentication;
using System;
using System.Collections.Generic;

namespace POS_API.Areas
{
    public class BaseController: ControllerBase
    {
        private Dictionary<string, string> _userInfo;
        protected readonly IAuthenticationUtilities _authenticationService;
        protected readonly ILogger<BaseController> _logger;
        public BaseController(ILogger<BaseController> logger, IAuthenticationUtilities authenticationService) 
        {
            _logger = logger;
            _authenticationService = authenticationService;
        }
        private bool DecryptClaimValues() 
        {
            Request?.Headers.TryGetValue("Authorization", out var token);
            if (!string.IsNullOrEmpty(token))
            {
                var claims = _authenticationService.GetClaims(token.ToString().Replace("Bearer ", "").Replace("bearer ", ""));
                _userInfo = claims;
                return true;
            }
            return false;
        }

        protected int COMPANY_ID
        {
            get 
            {
                try
                {
                    return _userInfo != null ? Convert.ToInt32(_userInfo[CustomClaimTypes.CompanyId]) :
                        DecryptClaimValues() ? Convert.ToInt32(_userInfo[CustomClaimTypes.CompanyId]) : 0;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        protected int? BRANCH_ID
        {
            get
            {
                try
                {
                    return _userInfo != null && _userInfo.ContainsKey(CustomClaimTypes.BranchId)
                        ? Convert.ToInt32(_userInfo[CustomClaimTypes.BranchId])
                        : DecryptClaimValues() && _userInfo.ContainsKey(CustomClaimTypes.BranchId)
                            ? (int?) Convert.ToInt32(_userInfo[CustomClaimTypes.BranchId])
                            : null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        protected int USER_ID
        {    
            get
            {
                try
                {
                    return _userInfo != null ? Convert.ToInt32(_userInfo[CustomClaimTypes.Id]) :
                        DecryptClaimValues() ? Convert.ToInt32(_userInfo[key: CustomClaimTypes.Id]) : 0;
                }
                catch (Exception ex )
                {
                    throw ex;
                }
                
            }
        }
    }
}
