using System;
using System.IO;
using System.Web;
using System.Net;
using System.Text;
using System.Diagnostics;

using PVKMT.Internals;

using PVKMT.Business.Interfaces.Services;
using PVKMT.Business.Interfaces.Infrastructure;

namespace PVKMT.Business.Services.SMS
{
    // SMSC smsc = new SMSC();
    // string[] r = smsc.send_sms("79999999999", "Ваш пароль: 123", 2);
    // string[] r = smsc.send_sms("79999999999", "http://smsc.ru\nSMSC.RU", 0, "", 0, 0, "", "maxsms=3");
    // string[] r = smsc.send_sms("79999999999", "0605040B8423F0DC0601AE02056A0045C60C036D79736974652E72750001036D7973697465000101", 0, "", 0, 5);
    // string[] r = smsc.send_sms("79999999999", "", 0, "", 0, 3);
    // string[] r = smsc.send_sms("dest@mysite.com", "Ваш пароль: 123", 0, 0, 0, 8, "source@mysite.com", "subj=Confirmation");
    // string balance = smsc.get_balance();

    /// <summary>
    /// SMS component
    /// </summary>
    internal class SMSC : ISmsGate
    {
        private static ILogger Log = Logger.Get(typeof(SMSC));

        // Константы с параметрами отправки
        private bool         SMSC_POST    = false;          // использовать метод POST
        private const string SMSC_SENDER  = "ПВК ЭТР МТ";   // Имя отправителя
        private const string SMSC_CHARSET = "utf-8";        // кодировка сообщения (windows-1251 или koi8-r), по умолчанию используется utf-8

        private IConfigurationService _config = null;

        public SMSC(IConfigurationService config)
        {
            _config = config;
        }

        // Метод отправки SMS
        // обязательные параметры:
        // phone - список телефонов через запятую или точку с запятой
        // message - отправляемое сообщение
        // необязательные параметры:
        // id - идентификатор сообщения. Представляет собой 32-битное число в диапазоне от 1 до 2147483647.
        // format - формат сообщения (0 - обычное sms, 1 - flash-sms, 2 - wap-push, 3 - hlr, 4 - bin, 5 - bin-hex, 6 - ping-sms, 7 - mms, 8 - mail, 9 - call)
        // sender - имя отправителя (Sender ID). Для отключения Sender ID по умолчанию необходимо в качестве имени передать пустую строку или точку.
        // возвращает массив строк (<id>, <количество sms>, <стоимость>, <баланс>) в случае успешной отправки
        // либо массив строк (<id>, -<код ошибки>) в случае ошибки
        public string[] SendSms(string phone, string message, int id)
        {
            string[] answer = _smsc_send_cmd("send", "cost=3&phones=" + _urlencode(phone)
                            + "&mes=" + _urlencode(message) + "%0A%0D" + _urlencode(SMSC_SENDER) + "&id=" + id.ToString() + "&translit=0");
            // (id, cnt, cost, balance) или (id, -error)
            if(answer.Length > 0)
            {
                if (answer.Length > 3 && answer[0] != "" && Convert.ToInt32(answer[1]) > 0)
                    _print_debug("Сообщение отправлено успешно. ID: " + answer[0] + ", всего SMS: " +
                        answer[1] + ", стоимость: " + answer[2] + ", баланс: " + answer[3]);
                else
                {
                    if(answer.Length > 1 && answer[1].Length > 0)
                        _print_debug("Ошибка №" + answer[1].Substring(1, 1) + (answer[0] != "0" ? ", ID: " + answer[0] : ""));
                }
            }
#if DEBUG
            answer = new string[] { id.ToString(), "1", "0", "0" };
#endif
            return answer;
        }
        // Метод получения баланса. 
        // Возвращает баланс в виде строки или пустую строку в случае ошибки
        public string GetBalance()
        {
            string[] answer = _smsc_send_cmd("balance", ""); // (balance) или (0, -error)
            if (answer.Length > 0)
            {
                if (answer.Length == 1)
                    _print_debug("Сумма на счете: " + answer[0]);
                else
                {
                    if (answer.Length > 1 && answer[1].Length > 0)
                        _print_debug("Ошибка №" + ((answer.Length > 0) ? answer[1].Substring(1, 1) : ""));
                }
            }
            return (answer.Length == 1 ? answer[0] : "");
        }

        // Метод вызова запроса. Формирует URL и делает 3 попытки чтения
        private string[] _smsc_send_cmd(string cmd, string arg)
        {
            string url, _url;

            arg = "login=" + _urlencode(_config.SMS.Login) + "&psw=" + _urlencode(_config.SMS.Password) + "&fmt=1&charset=" + SMSC_CHARSET + "&" + arg;
            url = _url = (_config.SMS.UseHTTPS ? "https" : "http") + "://smsc.ru/sys/" + cmd + ".php" + (SMSC_POST ? "" : "?" + arg);

            string ret;
            int i = 0;

            do
            {
                if (i++ > 0)
                    url = _url.Replace("smsc.ru/", "www" + i.ToString() + ".smsc.ru/");

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                if (_config.SMS.UseHTTPS)
                {
                    if (!String.IsNullOrEmpty(_config.PROXY.HTTPSAddress))
                    {
                        WebProxy proxy = new WebProxy(_config.PROXY.HTTPSAddress, _config.PROXY.HTTPSPort);
                        if (!String.IsNullOrEmpty(_config.PROXY.UserName))
                            proxy.Credentials = new NetworkCredential(_config.PROXY.UserName, _config.PROXY.UserPassword);
                        else
                            proxy.UseDefaultCredentials = true;
                        request.Proxy = proxy;
                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(_config.PROXY.HTTPAddress))
                    {
                        WebProxy proxy = new WebProxy(_config.PROXY.HTTPAddress, _config.PROXY.HTTPPort);
                        if (!String.IsNullOrEmpty(_config.PROXY.UserName))
                            proxy.Credentials = new NetworkCredential(_config.PROXY.UserName, _config.PROXY.UserPassword);
                        else
                            proxy.UseDefaultCredentials = true;
                        request.Proxy = proxy;
                    }
                }

                // reset answer
                ret = String.Empty;

                try
                {
                    if (SMSC_POST)
                    {
                        request.Method = "POST";
                        byte[] output = new byte[0];
                        request.ContentType = "application/x-www-form-urlencoded";
                        output = Encoding.UTF8.GetBytes(arg);
                        request.ContentLength = output.Length;
#if !DEBUG
                        using (Stream requestStream = request.GetRequestStream())
                            requestStream.Write(output, 0, output.Length);
#endif
                    }
#if !DEBUG
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                        ret = sr.ReadToEnd();
#endif
                }
                catch (WebException ex)
                {
                    Log.Debug("SMS.Send() failed.", ex);
                    ret = "";
                }
            }
            while (ret == "" && i < 5);

            if (ret == "")
            {
                _print_debug("Ошибка чтения адреса: " + url);
                ret = ","; // фиктивный ответ
            }

            char delim = ',';
            if (cmd == "status")
            {
                string[] par = arg.Split('&');
                for (i = 0; i < par.Length; i++)
                {
                    string[] lr = par[i].Split("=".ToCharArray(), 2);

                    if (lr[0] == "id" && lr[1].IndexOf("%2c") > 0) // запятая в id - множественный запрос
                        delim = '\n';
                }
            }
            return ret.Split(delim);
        }
        // кодирование параметра в http-запросе
        private string _urlencode(string str)
        {
            return SMSC_POST ? str : HttpUtility.UrlEncode(str);
        }
        // объединение байтовых массивов
        private byte[] _concatb(byte[] farr, byte[] sarr)
        {
            int opl = farr.Length;

            Array.Resize(ref farr, farr.Length + sarr.Length);
            Array.Copy(sarr, 0, farr, opl, sarr.Length);

            return farr;
        }
        // вывод отладочной информации
        private void _print_debug(string str)
        {
            Debug.WriteLine(str);
        }
    }
}
