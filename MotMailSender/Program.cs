using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//using NLog;

namespace MotMailSender
{
    class Program
    {
        static void Main(string[] args)
        {
            var mySender = new SmtpSender();
        }
    }
    public class SmtpSender
    {
        //private static Logger logger = LogManager.GetCurrentClassLogger();

        private bool _sendingComplite;
        private string _mailFrom;
        private SmtpClient _smtpClient;

        public SmtpSender()
        {
        }

        //private void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        //{
        //    var message = (Message)e.UserState;

        //    if (e.Cancelled)
        //        message.Status = "Сancelled";

        //    if (e.Error != null)
        //        message.Status = string.Format("Error: {0}", e.Error.ToString());
        //    else
        //    {
        //        message.Status = "Complited";
        //        message.WasSent = true;
        //    }

        //    _sendingComplite = true;
        //}

        public void SendMessage()
        {
            var mailMessage = new MailMessage()
            {
                From = new MailAddress(_mailFrom),
                Subject = message.Subject,
                IsBodyHtml = true,
                Body = message.Body
            };

            mailMessage.To.Add(new MailAddress(message.Recipient));
            mailMessage.Bcc.Add(new MailAddress(message.HiddenCopyMail));

            try
            {
                _sendingComplite = false;
                _smtpClient.SendAsync(mailMessage, message);

                while (!_sendingComplite)
                    Thread.Sleep(50);

                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                var str = string.Format("Sending message to {0} error: {1}", message.Recipient, ex.Message);
                logger.Error(str);

#if DEBUG
                Console.WriteLine(str);
#endif
            }
        }

        public void CreateSmtpClient(string host, int port, string mailFrom, string login, string password, bool enableSsl)
        {
            _mailFrom = mailFrom;

            _smtpClient = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(login, password),
                EnableSsl = enableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            _smtpClient.SendCompleted += SendCompletedCallback;
        }
    }
}
