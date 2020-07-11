﻿using Microsoft.AspNetCore.Authorization;


namespace WebAPI.Configure.AuthHandler
{

    public class MinimumAgeRequirement : IAuthorizationRequirement
    {
        public int MinimumAge { get; }

        public MinimumAgeRequirement(int minimumAge)
        {
            MinimumAge = minimumAge;
        }

    }
}