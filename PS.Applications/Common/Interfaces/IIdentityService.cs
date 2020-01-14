﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PS.Applications.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<string> GetUserNameAsync(string userId);

        //Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password);

        //Task<Result> DeleteUserAsync(string userId);
    }
}
