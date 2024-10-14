using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using POS_TECH_FASE_UM.Interface;
using POS_TECH_FASE_UM.Models;

namespace POS_TECH_FASE_UM.Pages.Contatos
{
    public class CreateModel : PageModel
    {
        private readonly IContatoService _contatoService;

        public CreateModel(IContatoService contatoService)
        {
            _contatoService = contatoService;
        }

        [BindProperty]
        public Contato Contato { get; set; }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _contatoService.AddContato(Contato);
            return RedirectToPage("./Index");
        }
    }
}
