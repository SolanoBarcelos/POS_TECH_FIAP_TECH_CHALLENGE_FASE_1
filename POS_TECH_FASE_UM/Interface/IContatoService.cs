using POS_TECH_FASE_UM.Models;

namespace POS_TECH_FASE_UM.Interface
{
    public interface IContatoService
    {
        IEnumerable<Contato> GetAllContatos();
        Contato GetContatoById(int id);
        IEnumerable<Contato> GetContatosByDDD(string ddd);
        void AddContato(Contato contato);
        void UpdateContato(Contato contato);
        bool DeleteContato(int id);
    }
}
