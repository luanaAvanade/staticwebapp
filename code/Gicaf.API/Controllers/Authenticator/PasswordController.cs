using System.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gicaf.API.Authentication.HttpClientSample;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using VGPartner.Infra.CrossCutting.Utils.RazorMailer;

namespace Gicaf.API.Controllers.Authenticator
{
    //[Route("api/[controller]")]
    [ApiController]
    public class PasswordController : ControllerBase
    {
        AuthenticatorClient _authenticator = new AuthenticatorClient(AuthenticatorSettings.UrlBase);
        public PasswordController()
        {
        }

        [HttpPost]
        //[AllowAnonymous]
        [Route("api/Password/Reset")]
        public async Task<object> Reset([FromBody]Login login)
        {
            var result = await _authenticator.ResetPassword(login);
            var token = JObject.FromObject(result).SelectToken("token").ToObject<string>();
            
            var linkConfirmacao = login.LinkConfirmacao.Replace("{userName}", login.Id).Replace("{token}", HttpUtility.UrlEncode(token));
            var confirmacaoEmail = new ConfirmacaoEmail{Nome = login.Id, LinkConfirmacao =linkConfirmacao };

            var message = EmailManager.MailerEngine.CreateMessage("TrocaEmail", confirmacaoEmail , login.Id, "Troca de Senha de Acesso");
            var email = EmailManager.MailerEngine.SendAsync(message);

            return result;
        }

        [HttpPost]
        //[AllowAnonymous]
        [Route("api/Password/Change")]
        public async Task<object> Change([FromBody]ResetPassword resetPassword)
        {
            return await _authenticator.ChangePassword(resetPassword, Request);
        }

    }

    public class ConfirmacaoEmail
    {
        public string Nome { get; set; }
        public string LinkConfirmacao { get; set; }
    }
}