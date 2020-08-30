using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotNet_API.Auth.Requirements
{
    public class AdminRequirement : IAuthorizationRequirement
    {
        public bool isAdmin { get; set; }

        public AdminRequirement(bool isAdmin)
        {
            this.isAdmin = isAdmin;
        }
    }
}