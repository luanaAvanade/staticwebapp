using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gicaf.API.Authentication;
using Gicaf.API.Authentication.HttpClientSample;
using Gicaf.Domain.Entities;
using Gicaf.Domain.Interfaces.Services;
using Gicaf.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Gicaf.API.Controllers.Authenticator
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        AuthenticatorClient _authenticator = new AuthenticatorClient(AuthenticatorSettings.UrlBase);
        IServiceBase<Usuario> _usuarioService;
        public LoginController(IServiceBase<Usuario> usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<object> Post([FromBody]Login login)
        {
            JObject jsonResult = null;
            // Ao logar no sistema o usuário do AD é feito um auto cadastro.
            // Assim sendo necessário a adição de uma role Padrão para esses usuários
            ICollection<string> Roles = new List<string>(){"Visualiza Sistema"};
            login.Roles = Roles;
            try
            {
                jsonResult = await _authenticator.LoginAsync(login) as JObject;
            }
            catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { ErrorMessage = e.Message });
            }
            
            var autResult = jsonResult.ToObject<AutResult>();

            if (autResult.newUser)
            {
                _usuarioService.Add(new Usuario { Nome = autResult.user.name, Email = autResult.user.email }, null);
                _usuarioService.SaveChanges();
            }
            var usuario = _usuarioService.GetWhere(null, x => x.Email == autResult.user.email).FirstOrDefault();
            return new { AppUser = usuario, AutResult = jsonResult };
        }
    }
}