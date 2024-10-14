namespace POS_TECH_FASE_UM.Repository
{
    using Dapper;
    using Dapper.Contrib.Extensions;
    using POS_TECH_FASE_UM.Interface;
    using POS_TECH_FASE_UM.Models;
    using System.Data;
    using System.Data.Common;

    public class ContatoRepository : IContatoRepository
    {
        private readonly IDbConnection _connection;

        public ContatoRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public IEnumerable<Contato> GetAll()
        {
            return _connection.GetAll<Contato>();
        }

        public Contato GetById(int id_contato)
        {
            //return _connection.Get<Contato>(id_contato);
            var query = "SELECT * FROM Contatos WHERE id_contato = @id_contato";
            return _connection.QuerySingleOrDefault<Contato>(query, new { id_contato });
        }

        public IEnumerable<Contato> GetByDDD(string ddd)
        {
            var query = "SELECT * FROM contatos WHERE telefone_contato LIKE @DDD";
            return _connection.Query<Contato>(query, new { DDD = $"{ddd}%" });
        }

        public void Insert(Contato contato)
        {
            // Pega o último contato do banco de dados
            var ultimoContatoQuery = "SELECT * FROM contatos ORDER BY id_contato DESC LIMIT 1";
            var ultimoContato = _connection.QuerySingleOrDefault<Contato>(ultimoContatoQuery);

            // Incrementa o ID do novo contato baseado no último ID encontrado
            contato.id_contato = (ultimoContato != null) ? ultimoContato.id_contato + 1 : 1;

            _connection.Insert(contato);
        }

        public void Update(Contato contato)
        {
            _connection.Update(contato);
        }

        public bool Delete(int id_contato)
        {
            var contato = _connection.Get<Contato>(id_contato);
            if (contato != null)
            {
                return _connection.Delete(contato);
            }
            return false;
        }
    }
}