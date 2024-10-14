using Microsoft.AspNetCore.Mvc;
using POS_TECH_FASE_UM.DTO;
using POS_TECH_FASE_UM.Interface;
using POS_TECH_FASE_UM.Models;
using System.Net.Mail;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace POS_TECH_FASE_UM.Controllers
{
    [ApiController]
    [Route("api/contatos")]
    public class ContatoController : ControllerBase
    {
        private readonly IContatoService _contatoService;

        public ContatoController(IContatoService contatoService)
        {
            _contatoService = contatoService;
        }

        // GET: api/contatos/up
        // Verifica se a API está no ar
        [HttpGet("up")]
        public IActionResult Up()
        {
            return Ok("API is running");
        }

        // GET: api/contatos/GetAll
        // Retorna todos os contatos
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(_contatoService.GetAllContatos());
        }

        // GET: api/contatos/GetById/{id_contato}
        // Retorna um contato específico pelo ID
        [HttpGet("GetById/{id_contato}")]
        public IActionResult GetById(int id_contato)
        {
            var contato = _contatoService.GetContatoById(id_contato);
            if (contato == null)
                return NotFound();

            return Ok(contato);
        }

        // GET: api/contatos/GetContatosByDDD/{ddd}
        // Retorna todos os contatos pelo DDD informado
        [HttpGet("GetContatosByDDD/{ddd}")]
        public IActionResult GetByDDD(string ddd)
        {
            return Ok(_contatoService.GetContatosByDDD(ddd));
        }

        // POST: api/contatos
        // Cria um novo contato com ID incrementado automaticamente
        [HttpPost]
        public IActionResult Create([FromBody] Contato contato)
        {
            _contatoService.AddContato(contato);
            return CreatedAtAction(nameof(GetById), new { id_contato = contato.id_contato }, contato);
        }

        // PATCH: api/contatos/Update/{id_contato}
        // Atualiza parcialmente um contato existente pelo ID
        [HttpPatch("Update/{id_contato}")]
        public IActionResult Update(int id_contato, [FromBody] ContatoDTO contatoDto)
        {
            var dadosContato = _contatoService.GetContatoById(id_contato);
            if (dadosContato == null)
                return NotFound(new { message = "Contato não encontrado." });

            var contatoAtualizado = _contatoService.UpdateContato(id_contato, contatoDto);

            return Ok(new
            {
                message = "Contato atualizado com sucesso.",
                contato = contatoAtualizado
            });
        }

        // DELETE: api/contatos/Delete/{id}
        // Exclui um contato pelo ID
        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var result = _contatoService.DeleteContato(id);
            if (result)
            {
                return Ok(new { message = "Contato deletado com sucesso." });
            }
            return NotFound(new { message = "Contato não encontrado." });
        }
    }
}