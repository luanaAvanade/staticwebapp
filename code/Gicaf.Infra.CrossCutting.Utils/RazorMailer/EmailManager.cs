using RazorMailer.Core;
using RazorMailer.Core.Dispatchers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace VGPartner.Infra.CrossCutting.Utils.RazorMailer
{
    public class EmailConfig
    {
        public string BaseUrl { get; set; }
        public string FromEmailAddress { get; set; }
        public string TestEmailAddress { get; set; }
        public SmtpConfig Smtp { get; set; }
       
    }
    public class SmtpConfig {
        public string Host { get; set; }
        public int? Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool UseDefaultCredentials { get; set; }

    }

    public static class EmailManager
    {
        public static RazorMailerEngine MailerEngine { get; set; }

        public static RazorMailerEngine Create(EmailConfig emailConfig)
        {
            MailerEngine = RazorMailerEngine.Create(emailConfig);
            return MailerEngine;
        }
    }
}
