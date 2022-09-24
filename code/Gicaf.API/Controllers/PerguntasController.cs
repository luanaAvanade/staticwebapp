using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gicaf.API.Controllers.Base;
using Gicaf.Application.Interface.DTOs;
using Gicaf.Application.Interface.Services;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Gicaf.API.Controllers
{
    public class PerguntasController : BaseController<IPerguntaAppService, PerguntaDTO, long>
    {
        public PerguntasController(IPerguntaAppService appService) : base(appService)
        {
            
        }
    }


}
