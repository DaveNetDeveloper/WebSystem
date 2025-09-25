using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common
{
    public static class Roles // TODO
    {
        public const string Admin = "Admin";
        public const string User = "User";
        public const string Manager = "Manager";
    }

    public static class AuthenticationSchemes // TODO
    {
        public const string Admin = "Admin";
        public const string Test = "Test";
        public const string Default = "Bearer";
    }
    public static class Environments
    {
        public static string Development { get => "Development"; }
        public static string Test { get => "Test"; }
        public static string Staging { get => "Staging"; }
        public static string Production { get => "Production"; }
    }
} 