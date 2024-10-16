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
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations.Schema;

namespace TESTES_POS_TECH_FASE_UM.DBConnectionIntegrationTest
{
    public class DBConnectionIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly IConfiguration _configuration;

        public DBConnectionIntegrationTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    // Carrega o arquivo de configurações apropriado para o ambiente de teste
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                });

                builder.ConfigureServices(services =>
                {
                    // Caso seja necessário configurar serviços específicos para o ambiente de teste, faça isso aqui.
                });
            });

            // Obtendo o IConfiguration a partir do contêiner de serviços configurado
            using (var scope = _factory.Services.CreateScope())
            {
                _configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            }
        }

        [Fact]
        public void Can_Connect_To_Database_And_Get_All_Contatos()
        {
            // Arrange
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            // Act & Assert
            using (IDbConnection dbConnection = new NpgsqlConnection(connectionString))
            {
                dbConnection.Open();

                var contatos = dbConnection.Query<Contato>("SELECT * FROM contatos");

                Assert.NotNull(contatos);
                Assert.NotEmpty(contatos);

                foreach (var contato in contatos)
                {
                    Assert.True(contato.id_contato > 0);
                }
            }
        }
    }

    // Classe modelo para representar os Contatos
    [Dapper.Contrib.Extensions.Table("contatos")]
    public class Contato
    {
        [Column("id_contato")]
        public int id_contato { get; set; }

        [Column("nome_contato")]
        public required string nome_contato { get; set; }

        [Column("telefone_contato")]
        public string telefone_contato { get; set; }

        [Column("email_contato")]
        public string email_contato { get; set; }
    }
}
