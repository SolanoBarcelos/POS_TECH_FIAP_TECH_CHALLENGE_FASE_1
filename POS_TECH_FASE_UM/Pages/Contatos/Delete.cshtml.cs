using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using POS_TECH_FASE_UM.Interface;
using POS_TECH_FASE_UM.Models;

namespace POS_TECH_FASE_UM.Pages.Contatos
{
    public class DeleteModel : PageModel
    {
        private readonly IContatoService _contatoService;

        public DeleteModel(IContatoService contatoService)
        {
            _contatoService = contatoService;
        }

        [BindProperty]
        public Contato Contato { get; set; }

        public IActionResult OnGet(int id)
        {
            Contato = _contatoService.GetContatoById(id);
            if (Contato == null)
            {
                return NotFound();
            }
            return Page();
        }

        public IActionResult OnPost(int id)
        {
            var result = _contatoService.DeleteContato(id);
            if (!result)
            {
                return NotFound();
            }
            return RedirectToPage("./Index");
        }
    }
}