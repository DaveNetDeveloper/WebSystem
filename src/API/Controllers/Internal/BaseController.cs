using Application.Common;
using Application.Interfaces.Services;
using Application.Interfaces.Common;
 
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text;

using UAParser;

namespace API.Controllers
{
    public class BaseController<TEntity> : ControllerBase
    {
        protected IConfiguration _config;
        protected ITokenService _tokenService;
        protected ILogger<BaseController<TEntity>> _logger;
        protected string _headerToken => HttpContext.Request.Headers["Authorization"].ToString();
        protected string ip => HttpContext.Connection.RemoteIpAddress?.ToString();
        protected string userAgent => HttpContext.Request.Headers["User-Agent"].ToString();
        protected Parser uaParser => Parser.GetDefault();
        protected ClientInfo clientInfo => uaParser.Parse(userAgent);
        protected UserAgent browser => clientInfo.UA;
        protected string os => clientInfo.OS.Family;
        protected string device => clientInfo.Device.Family;
        protected string primaryLanguage => GetPrimaryLanguage(HttpContext);

        private string GetPrimaryLanguage(HttpContext context)
        {
            var acceptLanguage = context.Request.Headers["Accept-Language"].ToString();

            if (string.IsNullOrWhiteSpace(acceptLanguage))
                return "unknown";
             
            var firstLang = acceptLanguage.Split(',').FirstOrDefault();

            if (string.IsNullOrWhiteSpace(firstLang))
                return "unknown";

            firstLang = firstLang.Split(';').FirstOrDefault()?.Trim();

            try {
                var culture = new CultureInfo(firstLang);
                return culture.Name;
            }
            catch {
                return firstLang;
            }
        }

        protected bool IsValidToken() => _tokenService.ValidarToken(_headerToken);

        protected IQueryOptions<TEntity> GetQueryOptions(int? page, int? pageSize, string? orderBy, bool descending = false)
            => new QueryOptions<TEntity> {
                    Page = page,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    Descending = descending }; 

    }
}
