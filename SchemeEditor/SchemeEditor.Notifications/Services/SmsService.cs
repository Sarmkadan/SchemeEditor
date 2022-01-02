using System;
using System.Threading.Tasks;

using PVKMT.Internals;
using PVKMT.Business.Interfaces.Services;
using PVKMT.Business.Interfaces.Infrastructure;

using PVKMT.Business.Services.SMS;

namespace PVKMT.Business.Services
{
    public class SmsService : ISmsService
    {
        private readonly IConfigurationService _config  = null;

        public SmsService(IConfigurationService config)
        {
            this._config = config;
        }

        public void Dispose()
        {
            // Nothing to do
        }

        public async Task<string> GetBalanceAsync()
        {
            var smsGate = BuildSmsGate();
            if (smsGate == null)
                return String.Empty;

            return await Task.Run<string>(() =>
            {
                return smsGate.GetBalance();
            });
        }
        public async Task<bool> SendKeyAsync(string phone, string key)
        {
            // fix number
            phone = Formatter.PhoneNumber(phone);

            if (String.IsNullOrEmpty(phone) || String.IsNullOrEmpty(key))
                return false;

            var smsGate = BuildSmsGate();
            if (smsGate == null)
                return false;

            return await Task.Run<bool>(() =>
            {
                var smsID = KeyGenerator.GenerateRandomNumber();
                var sendResult = smsGate.SendSms(phone, "Код подтверждения: " + key, smsID);
                if (sendResult == null || sendResult.Length < 4)
                    return false;
                if (sendResult[0] != smsID.ToString())
                    return false;
                return (sendResult[1] == "1");
            });
        }

        protected virtual ISmsGate BuildSmsGate()
        {
            if (_config == null)
                return null;
            return new SMSC(_config);
        }
    }
}
