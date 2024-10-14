using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using POS_TECH_FASE_UM.Interface;
using POS_TECH_FASE_UM.Models;

namespace POS_TECH_FASE_UM.Pages.Contatos
{
    public class EditModel : PageModel
    {
        private readonly IContatoService _contatoService;

        public EditModel(IContatoService contatoService)
        {
            _contatoService = contatoService;
        }

        [BindProperty]
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

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _contatoService.UpdateContato(Contato);
            return RedirectToPage("./Index");
        }
    }
}
