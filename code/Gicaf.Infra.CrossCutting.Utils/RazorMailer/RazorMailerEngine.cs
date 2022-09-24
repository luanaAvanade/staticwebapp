using System;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using RazorMailer.Core.Dispatchers;
using VGPartner.Infra.CrossCutting.Utils.RazorMailer;
using System.Collections;
using System.Collections.Generic;

namespace RazorMailer.Core
{
    /// <summary>
    /// The core RazorMailer engine responsible for converting Razor templates into MailMessages
    /// </summary>
    public class RazorMailerEngine : IDisposable//RazorEngineHost, IDisposable
    {
        private readonly string _fromName;
        private readonly string _fromEmail;
        private readonly IEmailDispatcher _dispatcher;
        private readonly IRazorEngineService _service;

        private string _templatePath;
        private string _testEmailAddress;
        DynamicViewBag _viewBag;

        //private readonly IDictionary<string, ITemplateKey> _templatesDictionary;

        //string _libraryPath;

        /// <summary>
        /// Constructs a RazorMailerEngine instance responsible for converting Razor templates into either a MailMessage or string.
        /// <para /> N.B. As this class loads up templates from the file system, it should only be created once per instance of your application.
        /// </summary>
        /// <param name="templatePath">The path to load the Razor templates from.  e.g. @"email\templates".  The template's Build Action should be set to Content and the Copy to Output Directory flag set to Copy Always</param>
        /// <param name="fromEmail">The address the email is from. e.g. hello@yoursite.com</param>
        /// <param name="fromName">The name the email is from. e.g. Your Site</param>
        /// <param name="dispatcher">The method by which to send the email.  Custom dispatchers can be implemented using the IEmailDispatcher interface</param>
        private RazorMailerEngine(string templatePath, string fromEmail, string fromName, IEmailDispatcher dispatcher, string baseUrl, string testEmailAddress = null)
        {
            _templatePath = templatePath;
            _fromName = fromName;
            _fromEmail = fromEmail;
            _dispatcher = dispatcher;
            _testEmailAddress = String.IsNullOrWhiteSpace(testEmailAddress) ? null : testEmailAddress;
            //_baseUrl = baseUrl;

            // Find templates in a web application
            var webPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", templatePath);
            // Find templates from a unit test or console application

            var libraryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, templatePath);
            //_libraryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, templatePath);

            var config = new TemplateServiceConfiguration
            {
                //Debug = true,
                TemplateManager = new ResolvePathTemplateManager(new[] { webPath, libraryPath })
            };

            _service = RazorEngineService.Create(config);
            _viewBag = new DynamicViewBag();
            _viewBag.AddValue("BaseUrl", baseUrl);
            //_templatesDictionary = new Dictionary<string, ITemplateKey>();
        }

        private string GetTemplatePath(string template)
        {
            string webPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", _templatePath, template + ".cshtml");
            string libraryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _templatePath, template + ".cshtml");

            if (File.Exists(webPath))
            {
                return webPath;
            }

            return libraryPath;
            //return String.Format(@"{0}\{1}.cshtml",Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _templatePath), template);
        }

        public void CompileSomaTemplatesDefault()
        {
            CompileTemplatePartial("EmailFooter");
            //CompileTemplate("PendenciasContratosFornecedor");
        }

        public RazorMailerEngine CompileTemplate<T>(string template)
        {
            var vKey = _service.GetKey(template);
            _service.Compile(vKey, typeof(T));

            return this;
        }

        public void CompileTemplatePartial(string template)
        {
            var vKey = _service.GetKey(GetTemplatePath(template));
            _service.Compile(vKey);
            return;
        }

        public static RazorMailerEngine Create(EmailConfig emailConfig)
        {
            var engine = new RazorMailerEngine(@"RazorMailer\Templates_Soma", emailConfig.FromEmailAddress, "Soma", SmtpDispatcher.Create(emailConfig), emailConfig.BaseUrl, emailConfig.TestEmailAddress);
            engine.CompileSomaTemplatesDefault();
            return engine;
        }

        /// <summary>
        /// Creates a string from the specified template
        /// </summary>
        /// <param name="template">The name of the Razor template (without the extension)</param>
        /// <returns>A string containing the result of the template</returns>
        public virtual string CreateMessage(string template, params KeyValuePair<string, object>[] viewBagValues)
        {
            var key = _service.GetKey(template);
            return _service.Run(key, modelType: null, model: null, viewBag: GetViewBag(viewBagValues));
        }

        /// <summary>
        /// Creates a string from the specified template
        /// </summary>
        /// <param name="template">The name of the Razor template (without the extension)</param>
        /// <param name="model">A typed model containting data for the variables in your Razor template</param>
        /// <returns>A string containing the result of the template and model</returns>
        public virtual string CreateMessage<T>(string template, T model, params KeyValuePair<string, object>[] viewBagValues)
        {
            var key = _service.GetKey(template);
            return _service.Run(key, typeof(T), model, GetViewBag(viewBagValues));
        }

        /// <summary>
        /// Creates a MailMessage from the specified template
        /// </summary>
        /// <param name="template">The name of the Razor template (without the extension)</param>
        /// <param name="to">The email address of the person the email is to be sent to</param>
        /// <param name="subject">The email subject</param>
        /// <param name="attachments">Any attachments to be included in the email</param>
        /// <returns>A MailMessage</returns>
        public virtual MailMessage CreateMessage(string template, string to, string subject, Attachment[] attachments = null, params KeyValuePair<string, object>[] viewBagValues)
        {
            var key = _service.GetKey(template);
            var body = _service.Run(key, null, GetViewBag(viewBagValues));

            return CreateMailMessage(to, subject, body, attachments);
        }

        /// <summary>
        /// Creates a MailMessage from the specified template and model
        /// </summary>
        /// <param name="template">The name of the Razor template (without the extension)</param>
        /// <param name="model">A typed model containting data for the variables in your Razor template</param>
        /// <param name="to">The address the email is to be sent to</param>
        /// <param name="subject">The email subject</param>
        /// <param name="attachments">Any attachments to be included in the email</param>
        /// <returns>A MailMessage</returns>
        public virtual MailMessage CreateMessage<T>(string template, T model, string to, string subject, Attachment[] attachments = null, params KeyValuePair<string, object>[] viewBagValues)
        {
            var vKey = _service.GetKey(template);
            var body = _service.Run(vKey, typeof(T), model, GetViewBag(viewBagValues));
            return CreateMailMessage(to, subject, body, attachments);
        }

        private DynamicViewBag GetViewBag(KeyValuePair<string, object>[] viewBagValues)
        {
            if (viewBagValues != null && viewBagValues.Length > 0)
            {
                DynamicViewBag newViewBag = new DynamicViewBag(_viewBag);
                foreach (var vViewBagValue in viewBagValues)
                {
                    newViewBag.AddValue(vViewBagValue.Key, vViewBagValue.Value);
                }
                return newViewBag;
            }

            return _viewBag;
        }

        /// <summary>
        /// Sends a MailMessage using the IEmailDispatcher specified in the constructor
        /// </summary>
        /// <param name="message">The MailMessage to send</param>
        public virtual void Send(MailMessage message)
        {
            if (_dispatcher == null)
                throw new MissingEmailDispatcherException("This RazorMailerEngine instance was constructed without a IEmailDispatcher and thus can't send MailMessages");

            _dispatcher.Send(message);
            message.Dispose();
        }

        /// <summary>
        /// Sends a MailMessage asynchronously using the IEmailDispatcher specified in the constructor
        /// </summary>
        /// <param name="message">The MailMessage to send</param>
        public virtual async Task SendAsync(MailMessage message)
        {
            if (_dispatcher == null)
                throw new MissingEmailDispatcherException("This RazorMailerEngine instance was constructed without a IEmailDispatcher and thus can't send MailMessages");
            try
            {
                await _dispatcher.SendAsync(message);
            }
            catch (Exception e)
            {
                LogMailError(message,e).Start();
                throw e;
            }
            message.Dispose();
        }

        public virtual async Task LogMailError(MailMessage message,Exception e)//todo[iuri] apagar apos corrigir erros de email no ambiente cemig
        {
            string path = @"c:\temp\logSrmEmail.txt";
            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("Log Envio Email");
                    sw.WriteLine("");
                }
            }
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(.../../src/"Erro ao Enviar email {DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()}" );
                sw.WriteLine(.../../src/" para {message.To} assunto: {message.Subject}"  );
                sw.WriteLine(.../../src/" erro: {e.Message} inner: {ObterInnerMensage(e)}"  );
                sw.WriteLine(.../../src/" host: {Newtonsoft.Json.JsonConvert.SerializeObject(_dispatcher.GetConfig()) }"  );
                sw.WriteLine(.../../src/"----------------------------------------------------------------------"  );
            }
        }
        public string ObterInnerMensage(Exception e)
        {
            if (e.InnerException != null)
                return ObterInnerMensage(e.InnerException);
            return e.Message;
        }


        /// <summary>
        /// Creates a MailMessage with the fromEmail and fromName specified in the constructor
        /// </summary>
        /// <param name="to">The email address to whom the email will be addressed</param>
        /// <param name="subject">The subject of the email</param>
        /// <param name="body">The HTML body of the email</param>
        /// <param name="attachments">Any attachments to be included in the email</param>
        /// <returns></returns>
        public virtual MailMessage CreateMailMessage(string to, string subject, string body, Attachment[] attachments = null)
        {
            if (string.IsNullOrEmpty(_fromEmail))
                throw new MissingInformationException("This RazorMailerEngine instance was constructed without a 'From Email' and thus can't send MailMessages");

            if (string.IsNullOrEmpty(_fromEmail))
                throw new MissingInformationException("This RazorMailerEngine instance was constructed without a 'From Name' and thus can't send MailMessages");

            var message = new MailMessage
            {
                Subject = subject,
                IsBodyHtml = true,
                Body = body
            };
            message.To.Add(_testEmailAddress ?? to);
            message.From = string.IsNullOrEmpty(_fromName) ? new MailAddress(_fromEmail) : new MailAddress(_fromEmail, _fromName);

            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    message.Attachments.Add(attachment);
                }
            }

            return message;
        }

        /// <summary>
        /// Disposes of the underlying RazorEngine service
        /// </summary>
        public void Dispose()
        {
            _service.Dispose();
        }
    }
}
