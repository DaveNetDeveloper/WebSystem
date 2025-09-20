using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common
{
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string User = "User";
        public const string Manager = "Manager";
    }

    public static class AuthenticationSchemes
    {
        public const string Admin = "Admin";
        public const string Test = "Test";
        public const string Default = "Bearer";
    }
     
} 