using System;
using System.Text;

using System.Net;
using System.Net.Mail;

using PVKMT.Internals;

using PVKMT.Business.Interfaces.DTO;
using PVKMT.Business.Interfaces.Services;
using PVKMT.Business.Interfaces.Infrastructure;

namespace PVKMT.Business.Services.MAIL
{
    /// <summary>
    /// MAIL component
    /// </summary>
    internal class MAILC : IMailGate
    {
        private static ILogger Log = Logger.Get(typeof(MAILC));

        // Константы с параметрами отправки
        private const string MAIL_SENDER  = "ПВК ЭТР МТ";   // Имя отправителя
        private const string MAIL_CHARSET = "utf-8";        // кодировка текста сообщения

        private IConfigurationService _config = null;

        public MAILC(IConfigurationService config)
        {
            _config = config;
        }

        public SendMailResult SendMail(string body, string subject, params string[] addresses)
        {
            var result = SendMailResult.Empty;

            // set up proxy
            WebProxy proxy = null;
            if (_config.EMAIL.UseHTTPS)
            {
                if (!String.IsNullOrEmpty(_config.PROXY.HTTPSAddress))
                    proxy = new WebProxy(_config.PROXY.HTTPSAddress, _config.PROXY.HTTPSPort);
            }
            else
            {
                if (!String.IsNullOrEmpty(_config.PROXY.HTTPAddress))
                    proxy = new WebProxy(_config.PROXY.HTTPAddress, _config.PROXY.HTTPPort);                   
            }

            if (proxy != null)
            {
                if (!String.IsNullOrEmpty(_config.PROXY.UserName))
                    proxy.Credentials = new NetworkCredential(_config.PROXY.UserName, _config.PROXY.UserPassword);
                else
                    proxy.UseDefaultCredentials = true;
                // set default proxy
                WebRequest.DefaultWebProxy = proxy;
            }

            using (var client = new SmtpClient(_config.EMAIL.SMTPHost, _config.EMAIL.SMTPPort))
            {
                client.EnableSsl             = _config.EMAIL.UseHTTPS;
                client.DeliveryMethod        = SmtpDeliveryMethod.Network;
                client.DeliveryFormat        = SmtpDeliveryFormat.SevenBit;
                client.UseDefaultCredentials = false;
                client.Timeout               = 10000;  // 10 seconds
                client.Credentials           = new NetworkCredential(_config.EMAIL.SMTPLogin, _config.EMAIL.SMTPPassword);

                foreach (var target in addresses)
                {
                    try
                    {
                        var message = new MailMessage()
                        {
                            IsBodyHtml      = true,
                            Body            = body,
                            BodyEncoding    = Encoding.UTF8,
                            Subject         = subject,
                            SubjectEncoding = Encoding.UTF8,
                            From            = new MailAddress(_config.EMAIL.SMTPFrom, MAIL_SENDER)
                        };
                        message.To.Add(new MailAddress(target));
#if !DEBUG
                        client.Send(message);
#endif
                        result.Add(target, true);
                    }
                    catch (Exception ex)
                    {
                        Log.Debug("SmtpClient.Send() to " + target + " failed.", ex);
                        result.Add(target, false);
                        continue;
                    }
                }
            }
            return result;
        }
    }
}
