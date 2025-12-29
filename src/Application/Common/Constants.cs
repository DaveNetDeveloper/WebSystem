using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class Environments
    {
        public static string Development { get => "Development"; }
        public static string Test { get => "Test"; }
        public static string Staging { get => "Staging"; }
        public static string Production { get => "Production"; }
    }
} 