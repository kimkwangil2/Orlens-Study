﻿using Microsoft.AspNetCore.Identity;
using PS.Applications.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PS.Infrasture.Identity
{
    public class IdentityService : IIdentityService
    {
        //private readonly UserManager<ApplicationUser> _userManager;

        public IdentityService()//UserManager<ApplicationUser> userManager)
        {
            //  _userManager = userManager;
        }
        public async Task<string> GetUserNameAsync(string userId)
        {
            //var user = await _userManager.Users.FirstAsync(u => u.Id == userId);
            await Task.Delay(100);
            return "";
        }
         
    }
}
