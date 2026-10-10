using AutoMapper;
using Fiap.Api.Reciclagem.Models;
using Fiap.Api.Reciclagem.Services;
using Fiap.Api.Reciclagem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.Api.Reciclagem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DescarteController : ControllerBase
    {
        private readonly IDescarteService _service;
        private readonly IMapper _mapper;

        public DescarteController(IDescarteService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<DescarteViewModel>> Get(
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanho = 10,
            [FromQuery] int? pontoColetaId = null)
        {
            var descartes = _service.Listar(pagina, tamanho, pontoColetaId);
            return Ok(_mapper.Map<IEnumerable<DescarteViewModel>>(descartes));
        }

        [HttpGet("{id}")]
        public ActionResult<DescarteViewModel> Get(int id)
        {
            var descarte = _service.ObterPorId(id);
            if (descarte == null)
                return NotFound();

            return Ok(_mapper.Map<DescarteViewModel>(descarte));
        }

        // Endpoint avançado: ao registrar o descarte, a carga do ponto de
        // coleta é atualizada e, se o limite configurado for atingido, um
        // alerta automático é gerado (ver DescarteService).
        [HttpPost]
        [Authorize(Roles = "operador,gestor")]
        public ActionResult Post([FromBody] DescarteCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var descarte = _mapper.Map<Descarte>(viewModel);
                var (criado, alerta) = _service.RegistrarDescarte(descarte);

                var resposta = new
                {
                    Descarte = _mapper.Map<DescarteViewModel>(criado),
                    AlertaGerado = alerta != null
                };

                return CreatedAtAction(nameof(Get), new { id = criado.DescarteId }, resposta);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }
    }
}
