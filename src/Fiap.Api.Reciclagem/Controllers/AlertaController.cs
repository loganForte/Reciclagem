using AutoMapper;
using Fiap.Api.Reciclagem.Services;
using Fiap.Api.Reciclagem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.Api.Reciclagem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlertaController : ControllerBase
    {
        private readonly IAlertaService _service;
        private readonly IMapper _mapper;

        public AlertaController(IAlertaService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<AlertaViewModel>> Get(
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanho = 10,
            [FromQuery] bool? resolvido = null)
        {
            var alertas = _service.Listar(pagina, tamanho, resolvido);
            return Ok(_mapper.Map<IEnumerable<AlertaViewModel>>(alertas));
        }

        [HttpGet("{id}")]
        public ActionResult<AlertaViewModel> Get(int id)
        {
            var alerta = _service.ObterPorId(id);
            if (alerta == null)
                return NotFound();

            return Ok(_mapper.Map<AlertaViewModel>(alerta));
        }

        // Marca o alerta como resolvido (caminhão de coleta já passou no ponto)
        [HttpPut("{id}/resolver")]
        [Authorize(Roles = "gestor")]
        public ActionResult Resolver(int id)
        {
            var resolvido = _service.Resolver(id);
            if (!resolvido)
                return NotFound();

            return NoContent();
        }
    }
}
