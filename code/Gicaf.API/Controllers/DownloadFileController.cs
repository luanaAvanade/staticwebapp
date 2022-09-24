using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Gicaf.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gicaf.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DownloadFileController : ControllerBase
    {
        IArquivoService _arquivoService;

        public DownloadFileController(IArquivoService arquivoService)
        {
            _arquivoService = arquivoService;
        }


        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var arquivo = _arquivoService.Get(id);
            var conteudo = arquivo.Conteudo.FirstOrDefault();

            return new FileContentResult(conteudo, "application/octet-stream")
            {
                FileDownloadName = arquivo.NomeArquivo
            };
        }
    }
}