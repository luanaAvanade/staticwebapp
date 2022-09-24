using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gicaf.API.Authentication.HttpClientSample;
using Gicaf.Domain.Entities;
using Gicaf.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gicaf.API.Controllers.Authenticator
{
    //[Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        AuthenticatorClient _authenticator = new AuthenticatorClient(AuthenticatorSettings.UrlBase);
        IServiceBase<Usuario> _usuarioService;
        public UserController(IServiceBase<Usuario> usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost]
        //[AllowAnonymous]
        [Route("api/User/EmailConfirmationValidation")]
        public async Task<object> Change([FromBody]EmailConfirmation emailConfirmation)
        {
            return await _authenticator.EmailConfirmationValidation(emailConfirmation, Request);
        }

        [HttpGet]
        //[AllowAnonymous]
        [Route("api/User/GetUsersInRole/{role}")]
        public async Task<IEnumerable<Usuario>> GetUsersInRole(string role)
        {
            var autUserNames = (await _authenticator.GetUsersInRole(role));
            return _usuarioService.GetWhere(null, x => autUserNames.Contains(x.Email));
        }
    }
}