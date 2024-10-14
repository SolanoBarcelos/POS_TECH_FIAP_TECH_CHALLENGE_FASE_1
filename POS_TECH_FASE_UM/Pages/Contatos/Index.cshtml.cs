using Microsoft.AspNetCore.Mvc.RazorPages;
using POS_TECH_FASE_UM.Interface;
using POS_TECH_FASE_UM.Models;
using System.Collections.Generic;

namespace POS_TECH_FASE_UM.Pages.Contatos
{
    public class IndexModel : PageModel
    {
        private readonly IContatoService _contatoService;

        public IndexModel(IContatoService contatoService)
        {
            _contatoService = contatoService;
        }

        public IEnumerable<Contato> Contatos { get; set; }

        public void OnGet()
        {
            Contatos = _contatoService.GetAllContatos();
        }
    }
}