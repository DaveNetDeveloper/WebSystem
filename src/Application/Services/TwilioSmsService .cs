using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;

using Microsoft.Extensions.Options;

using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Application.Services
{
    public class TwilioSmsService : ISmsService
    {
        private readonly SmsConfiguration _smsConfig;
         
        public TwilioSmsService(IOptions<SmsConfiguration> smsConfig)
        {
            _smsConfig = smsConfig.Value;
            TwilioClient.Init(_smsConfig.AccountSid, _smsConfig.AuthToken);
        }

        public async Task SendAsync(string to, string message)
        {
            await MessageResource.CreateAsync(
                body: message,
                from: new Twilio.Types.PhoneNumber(_smsConfig.FromNumber),
                to: new Twilio.Types.PhoneNumber(to)
            );
        }
    }
}