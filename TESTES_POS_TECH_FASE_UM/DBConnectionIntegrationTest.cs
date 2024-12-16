using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Npgsql;
using POS_TECH_FASE_UM;
using Microsoft.Extensions.Configuration;

namespace TESTES_POS_TECH_FASE_UM.DataBaseIntegrationTests
{
    public class DBConnectionTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly IConfiguration _configuration;

        public DBConnectionTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;

            // Obtenha o serviço IConfiguration do WebApplicationFactory
            _configuration = _factory.Services.GetService(typeof(IConfiguration)) as IConfiguration;
        }

        [Fact]
        public async Task Can_Connect_To_Database_And_Get_All_Contatos()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Obtenha a connection string dinamicamente do appsettings.json
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            // Act & Assert
            try
            {
                using (IDbConnection dbConnection = new NpgsqlConnection(connectionString))
                {
                    dbConnection.Open();

                    // Consulta SQL para pegar todos os contatos
                    var contatos = await dbConnection.QueryAsync<Contato>("SELECT * FROM contatos");

                    // Verifica se o resultado não é nulo
                    Assert.NotNull(contatos);

                    // Verifica se há contatos retornados
                    Assert.NotEmpty(contatos);

                    foreach (var contato in contatos)
                    {
                        // Verifica se os campos de cada contato estão preenchidos adequadamente
                        Assert.True(contato.id_contato > 0); // Verifica se o Id é válido
                    }
                }
            }
            catch (Exception ex)
            {
                Assert.Null(ex); // Garante que nenhuma exceção foi lançada
            }
        }
    }

    // Classe modelo para representar os Contatos
    public class Contato
    {
        public int? id_contato { get; set; }
        public string? nome_contato { get; set; }
        public string? telefone_contato { get; set; }
        public string? email_contato { get; set; }
    }
}
