using POS_TECH_FASE_UM.Models;

namespace POS_TECH_FASE_UM.Interface
{
    public interface IContatoRepository
    {
        IEnumerable<Contato> GetAll();
        Contato GetById(int id_contato);
        IEnumerable<Contato> GetByDDD(string ddd);
        void Insert(Contato contato);
        void Update(Contato contato);
        bool Delete(int idContato);
    }
}
