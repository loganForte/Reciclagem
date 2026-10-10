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
    public class PontoColetaController : ControllerBase
    {
        private readonly IPontoColetaService _service;
        private readonly IMapper _mapper;

        public PontoColetaController(IPontoColetaService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<PaginacaoViewModel<PontoColetaViewModel>> Get([FromQuery] int pagina = 1, [FromQuery] int tamanho = 10)
        {
            var pontos = _service.Listar(pagina, tamanho);
            var viewModelList = _mapper.Map<IEnumerable<PontoColetaViewModel>>(pontos);

            var resultado = new PaginacaoViewModel<PontoColetaViewModel>
            {
                Itens = viewModelList,
                Pagina = pagina,
                Tamanho = tamanho,
                TotalItens = _service.ContarTotal()
            };

            return Ok(resultado);
        }

        [HttpGet("{id}")]
        public ActionResult<PontoColetaViewModel> Get(int id)
        {
            var ponto = _service.ObterPorId(id);
            if (ponto == null)
                return NotFound();

            return Ok(_mapper.Map<PontoColetaViewModel>(ponto));
        }

        [HttpPost]
        [Authorize(Roles = "gestor")]
        public ActionResult Post([FromBody] PontoColetaCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ponto = _mapper.Map<PontoColeta>(viewModel);
            var criado = _service.Criar(ponto);

            return CreatedAtAction(nameof(Get), new { id = criado.PontoColetaId }, _mapper.Map<PontoColetaViewModel>(criado));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "gestor")]
        public ActionResult Put(int id, [FromBody] PontoColetaCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ponto = _mapper.Map<PontoColeta>(viewModel);
            var atualizado = _service.Atualizar(id, ponto);

            if (!atualizado)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "gestor")]
        public ActionResult Delete(int id)
        {
            var removido = _service.Remover(id);
            if (!removido)
                return NotFound();

            return NoContent();
        }
    }
}
