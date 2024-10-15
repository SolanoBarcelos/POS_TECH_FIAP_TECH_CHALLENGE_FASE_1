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
using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations.Schema;

namespace TESTES_POS_TECH_FASE_UM.DataBaseIntegrationTest
{
    public class DBConnectionIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly IConfiguration _configuration;

        public DBConnectionIntegrationTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory;


            _configuration = _factory.Services.GetService(typeof(IConfiguration)) as IConfiguration;
        }

        [Fact]
        public async Task Can_Connect_To_Database_And_Get_All_Contatos()
        {
            // Arrange
            var client = _factory.CreateClient();

            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            // Act & Assert
            try
            {
                using (IDbConnection dbConnection = new NpgsqlConnection(connectionString))
                {
                    dbConnection.Open();


                    var contatos = await dbConnection.QueryAsync<Contato>("SELECT * FROM contatos");


                    Assert.NotNull(contatos);


                    Assert.NotEmpty(contatos);

                    foreach (var contato in contatos)
                    {
                        Assert.True(contato.id_contato > 0);
                    }
                }
            }
            catch (Exception ex)
            {
                Assert.Null(ex);
            }
        }
    }

    // Classe modelo para representar os Contatos
    [Dapper.Contrib.Extensions.Table("contatos")]
    public class Contato
    {
        [ExplicitKey]
        [Column("id_contato")]
        public int id_contato { get; set; }

        [Column("nome_contato")]
        public string nome_contato { get; set; }

        [Column("telefone_contato")]
        public string telefone_contato { get; set; }

        [Column("email_contato")]
        public string email_contato { get; set; }
    }
}

