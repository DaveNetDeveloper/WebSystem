using API.Controllers;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Core;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace API.Middlewares
{ 
    public class FakeJwtBearerHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IConfiguration _configuration;
        protected ILogger<FakeJwtBearerHandler> _logger;

        public FakeJwtBearerHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
                                    ILoggerFactory logger,
                                    UrlEncoder encoder,
                                    ISystemClock clock,
                                    IConfiguration configuration)
            : base(options, logger, encoder, clock) {
           
            Console.Out.WriteLine("FakeJwtBearerHandler"); 
            _configuration = configuration;
            _logger = logger.CreateLogger<FakeJwtBearerHandler>(); 
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {  
            var validToken = _configuration["TestAuth:ValidToken"];

            if (Request.Headers.TryGetValue("Authorization", out var authHeader) && authHeader.ToString().Replace("Bearer ", "") == validToken) {

                var claims = new[] {
                    new Claim(ClaimTypes.Name, "TestAdmin"),
                    new Claim(ClaimTypes.Role, "Admin") };

                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);
 
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }   
            return Task.FromResult(AuthenticateResult.Fail("Invalid Token"));
        }
    } 
}