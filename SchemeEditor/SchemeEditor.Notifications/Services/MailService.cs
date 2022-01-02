using System;
using System.Linq;
using System.Threading.Tasks;

using PVKMT.Internals;

using PVKMT.Business.Interfaces.DTO;
using PVKMT.Business.Interfaces.Services;
using PVKMT.Business.Interfaces.Infrastructure;

using PVKMT.Business.Services.MAIL;

namespace PVKMT.Business.Services
{
    public class MailService : IMailService
    {
        private readonly IConfigurationService _config  = null;

        public MailService(IConfigurationService config)
        {
            this._config = config;
        }

        public void Dispose()
        {
            // Nothing to do
        }

        public async Task<SendMailResult> SendMailAsync(string body, string subject, params string[] addresses)
        {
            // fix mail addresses
            var targets = addresses
                .Select(addr => Formatter.EMailAddress(addr))
                .Where(addr => !String.IsNullOrEmpty(addr)).ToArray();

            if (targets.Length == 0 || String.IsNullOrEmpty(body) || String.IsNullOrEmpty(subject))
                return SendMailResult.Empty;

            var gate = BuildMailGate();
            if (gate == null)
                return SendMailResult.Empty;

            return await Task.Run<SendMailResult>(() =>
            {
                return gate.SendMail(body, subject, targets);
            });
        }

        protected virtual IMailGate BuildMailGate()
        {
            if (_config == null)
                return null;
            return new MAILC(_config);
        }
    }
}
