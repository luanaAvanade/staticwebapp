using System;
using System.ComponentModel;
using System.Net.Mail;
using System.Threading.Tasks;
using VGPartner.Infra.CrossCutting.Utils.RazorMailer;

namespace RazorMailer.Core.Dispatchers
{
    /// <summary>
    /// A simple dispatcher that sends emails using the built in .NET SmtpClient with default settings (i.e using configuration file settings)
    /// </summary>
    public class SmtpDispatcher : IEmailDispatcher
    {
        public EmailConfig _emailConfig;
        string _userName;
        private SmtpDispatcher(EmailConfig  emailConfig)
        {
            _emailConfig = emailConfig;
            _userName = emailConfig.FromEmailAddress;
        }

        public static SmtpDispatcher Create(EmailConfig emailConfig)
        {
            return new SmtpDispatcher(emailConfig);
        }
        public EmailConfig GetConfig()
        {
            return _emailConfig;
        }
        private SmtpClient GetSmtpClient()
        {
            SmtpClient vSmtpClient = GetSomaSmtpConfig();
            vSmtpClient.SendCompleted += new
            SendCompletedEventHandler(SendCompletedCallback);
            return vSmtpClient;
        }

        private SmtpClient GetSomaSmtpConfig()
        {
            var client = new SmtpClient
            {
                Host = _emailConfig.Smtp.Host, //"srvsmtp01.axxiom1.local",
                Port = _emailConfig.Smtp.Port??0,
                UseDefaultCredentials = _emailConfig.Smtp.UseDefaultCredentials,
                EnableSsl = false,
            };
            if (!String.IsNullOrEmpty(_emailConfig.Smtp.UserName) && (!String.IsNullOrEmpty(_emailConfig.Smtp.Password)))
                client.Credentials = new System.Net.NetworkCredential(_emailConfig.Smtp.UserName, _emailConfig.Smtp.Password);
            return client;
        }

        /// <summary>
        /// Sends a MailMessage 
        /// </summary>
        /// <param name="message">The MailMessage to send</param>
        public void Send(MailMessage message)
        {
            using (var vSmtpCliente = GetSmtpClient())
            {
                vSmtpCliente.Send(message);
            }
            //_smtpClient.Send(message);
            //_smtpClient.Dispose();
        }

        /// <summary>
        /// Sends a MailMessage asynchronously 
        /// </summary>
        /// <param name="message">The MailMessage to send</param>
        public async Task SendAsync(MailMessage message)
        {
            using (var vSmtpCliente = GetSmtpClient())
            {
                await vSmtpCliente.SendMailAsync(message);
            }

            //using (var smtp = new SmtpClient())
            //{
            //    await smtp.SendMailAsync(message);
            //}
            //await _smtpClient.SendMailAsync(message);
        }

        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            String token = e.UserState.ToString();

            if (e.Cancelled)
            {
                //Console.WriteLine("[{0}] Send canceled.", token);
                //((SmtpDispatcher)sender).Error = new Exception("Send canceled");
                throw new Exception("Send canceled");
            }
            if (e.Error != null)
            {
                //Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
                //((SmtpDispatcher)sender).Error = e.Error;
                //throw e.Error;
            }
            else
            {
                Console.WriteLine("Message sent.");
            }
            //_mailSent = true;
        }
    }
}
