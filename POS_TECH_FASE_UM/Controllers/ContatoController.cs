using Microsoft.AspNetCore.Mvc;
using POS_TECH_FASE_UM.Interface;
using POS_TECH_FASE_UM.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace POS_TECH_FASE_UM.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContatoController : ControllerBase
    {
        private readonly IContatoService _contatoService;

        public ContatoController(IContatoService contatoService)
        {
            _contatoService = contatoService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_contatoService.GetAllContatos());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id_contato)
        {
            var contato = _contatoService.GetContatoById(id_contato);
            if (contato == null)
                return NotFound();

            return Ok(contato);
        }

        [HttpGet("ddd/{ddd}")]
        public IActionResult GetByDDD(string ddd)
        {
            return Ok(_contatoService.GetContatosByDDD(ddd));
        }

        [HttpPost]
        public IActionResult Create([FromBody] Contato contato)
        {
            _contatoService.AddContato(contato);
            return CreatedAtAction(nameof(GetById), new { id = contato.id_contato }, contato);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id_contato, [FromBody] Contato contato)
        {
            if (id_contato != contato.id_contato)
                return BadRequest();

            _contatoService.UpdateContato(contato);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _contatoService.DeleteContato(id);
            if (result)
            {
                return Ok();
            }
            return NotFound();
        }
    }
}
