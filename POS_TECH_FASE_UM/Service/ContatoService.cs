using POS_TECH_FASE_UM.DTO;
using POS_TECH_FASE_UM.Interface;
using POS_TECH_FASE_UM.Models;
using System.Text.RegularExpressions;

namespace POS_TECH_FASE_UM.Service
{
    public class ContatoService : IContatoService
    {
        private readonly IContatoRepository _repository;

        public ContatoService(IContatoRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Contato> GetAllContatos()
        {
            return _repository.GetAll();
        }

        public Contato GetContatoById(int id_contato)
        {
            return _repository.GetById(id_contato);
        }

        public IEnumerable<Contato> GetContatosByDDD(string ddd)
        {
            if (string.IsNullOrEmpty(ddd) || ddd.Length != 2)
            {
                throw new ArgumentException("DDD inválido");
            }

            return _repository.GetByDDD(ddd);
        }

        public void AddContato(Contato contato)
        {
            ValidateContato(contato, isPartialUpdate: false);

            _repository.Insert(contato);
        }

        public Contato UpdateContato(int id_contato, ContatoDTO contatoDto)
        {
    
            var validaContato = GetContatoById(id_contato);

            if (validaContato == null)
            {
                throw new Exception("Contato não encontrado");
            }

            // Atualiza somente os campos que foram enviados na requisição
            if (!string.IsNullOrEmpty(contatoDto.nome_contato))
            {
                validaContato.nome_contato = contatoDto.nome_contato;
            }

            if (!string.IsNullOrEmpty(contatoDto.telefone_contato))
            {
                validaContato.telefone_contato = contatoDto.telefone_contato;
            }

            if (!string.IsNullOrEmpty(contatoDto.email_contato))
            {
                validaContato.email_contato = contatoDto.email_contato;
            }

            // Valida o objeto atualizado
            ValidateContato(validaContato, isPartialUpdate: true);

            // Atualiza o registro no banco de dados
            _repository.Update(validaContato);

            // Retorna o objeto contato atualizado
            return validaContato;
        }


        public bool DeleteContato(int id_contato)
        {
            return _repository.Delete(id_contato);
        }

        //private void ValidateUpdateContato(Contato existingContato)
        //{
        //    if (!IsValidEmail(existingContato.email_contato))
        //    {
        //        throw new ArgumentException("E-mail inválido.");
        //    }

        //    if (!IsValidTelefone(existingContato.telefone_contato))
        //    {
        //        throw new ArgumentException("Telefone inválido.");

        //    }

        //}

        private void ValidateContato(Contato contato, bool isPartialUpdate = false)
        {
            if (!isPartialUpdate || !string.IsNullOrEmpty(contato.nome_contato))
            {
                if (string.IsNullOrEmpty(contato.nome_contato))
                {
                    throw new ArgumentException("Nome do contato é obrigatório.");
                }
            }

            if (!isPartialUpdate || !string.IsNullOrEmpty(contato.email_contato))
            {
                if (!IsValidEmail(contato.email_contato))
                {
                    throw new ArgumentException("E-mail inválido.");
                }
            }

            if (!isPartialUpdate || !string.IsNullOrEmpty(contato.telefone_contato))
            {
                if (!IsValidTelefone(contato.telefone_contato))
                {
                    throw new ArgumentException("Telefone inválido.");
                }
            }
        }

        private bool IsValidEmail(string email)
        {
            return !string.IsNullOrEmpty(email) && email.Contains("@");
        }

        private bool IsValidTelefone(string telefone)
        {
            var regex = new Regex(@"^\d{11}$");
            return regex.IsMatch(telefone);
        }
    }
}