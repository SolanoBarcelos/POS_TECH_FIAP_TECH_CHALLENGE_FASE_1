using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using POS_TECH_FASE_UM.Interface;
using POS_TECH_FASE_UM.Models;

namespace POS_TECH_FASE_UM.Pages.Contatos
{
    public class DetailsModel : PageModel
    {
        private readonly IContatoService _contatoService;

        public DetailsModel(IContatoService contatoService)
        {
            _contatoService = contatoService;
        }

        public Contato Contato { get; set; }

        public IActionResult OnGet(int id_contato)
        {
            Contato = _contatoService.GetContatoById(id_contato);
            if (Contato == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
