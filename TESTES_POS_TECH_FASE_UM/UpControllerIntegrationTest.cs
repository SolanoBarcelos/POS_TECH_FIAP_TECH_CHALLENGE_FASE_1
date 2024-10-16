using System.Net;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using POS_TECH_FASE_UM; // Certifique-se de que o namespace está correto
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;

namespace TESTES_POS_TECH_FASE_UM.UpControllerIntegrationTest
{
    public class UpControllerIntegrationTest : IClassFixture<WebApplicationFactory<Program>> // Substitua Program pelo nome correto se necessário
    {
        private readonly WebApplicationFactory<Program> _factory;

        public UpControllerIntegrationTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.UseContentRoot("../../../../POS_TECH_FASE_UM");
                builder.ConfigureServices(services =>
                {
                    // Caso seja necessário configurar serviços específicos para o ambiente de teste, faça isso aqui.
                });
            });
        }

        [Fact]
        public async Task Get_Up_ReturnsOk()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/contatos/up");

            // Assert
            response.EnsureSuccessStatusCode(); // Verifica se o status de sucesso (200-299) é retornado
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal("API is running", content);
        }
    }
}