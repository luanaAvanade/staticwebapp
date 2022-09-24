using System.Reflection.Metadata.Ecma335;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Gicaf.API.Authentication.HttpClientSample;
using Gicaf.Domain.Entities;
using Gicaf.Domain.Interfaces.Services;
using Gicaf.Infra.CrossCutting.IoC;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using VGPartner.Infra.CrossCutting.Utils.RazorMailer;

namespace Gicaf.API.Controllers.Authenticator
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        Authentication.HttpClientSample.AuthenticatorClient _authenticator = new Authentication.HttpClientSample.AuthenticatorClient(AuthenticatorSettings.UrlBase);
        IServiceBase<Usuario> _usuarioService;
        public RegisterController(IServiceBase<Usuario> usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<object> Post([FromBody]UserRegister user)
        {
            var usuario = new Usuario() { Nome = user.Name, Email = user.Email };
            _usuarioService.Add(usuario, null);
            _usuarioService.SaveChanges();
            var result = await _authenticator.RegisterUserAsync(user);
            var token = JObject.FromObject(result).SelectToken("result").ToObject<string>();
            //string baseUrl = ../../"{Request.Scheme}://{Request.Host}{Request.PathBase}";
            //"localhost:5000/confirmacao-cadastro-fornecedor?userName={userName}&token={token}";
            SendConfirmationEmail(user, usuario, token);
            return result;
        }

        private void SendConfirmationEmail(UserRegister user, Usuario usuario, string token)
        {
            var linkConfirmacao = user.LinkConfirmacao.Replace("{userName}", user.UserName).Replace("{token}", HttpUtility.UrlEncode(token));
            //var uri = new Uri(linkConfirmacao);
            var confirmacaoEmail = new ConfirmacaoEmail { Nome = usuario.Nome, LinkConfirmacao = linkConfirmacao };
            var message = EmailManager.MailerEngine.CreateMessage("BemVindo", confirmacaoEmail, usuario.Email, "Confirmação de Cadastro");
            var result = EmailManager.MailerEngine.SendAsync(message);
        }

    }
}