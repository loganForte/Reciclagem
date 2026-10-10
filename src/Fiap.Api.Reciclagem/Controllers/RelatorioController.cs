using Fiap.Api.Reciclagem.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.Api.Reciclagem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RelatorioController : ControllerBase
    {
        private readonly IRelatorioService _service;

        public RelatorioController(IRelatorioService service) => _service = service;

        // Relatório agregado: total coletado por tipo de resíduo, por ponto
        // de coleta e quantidade de alertas em aberto.
        [HttpGet("resumo")]
        public ActionResult Get()
        {
            return Ok(_service.ResumoGeral());
        }
    }
}
