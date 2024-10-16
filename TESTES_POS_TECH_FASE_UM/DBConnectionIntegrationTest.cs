using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace TESTES_POS_TECH_FASE_UM
{
    public class DBConnectionIntegrationTest
    {
        private readonly IWebHostBuilder _hostBuilder;
        private readonly string _contentRootPath;
        public string nome_contato { get; set; } = null!;
        public string telefone_contato { get; set; } = null!;
        public string email_contato { get; set; } = null!;

        public DBConnectionIntegrationTest()
        {
            _contentRootPath = Directory.GetCurrentDirectory();
            _hostBuilder = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseContentRoot(_contentRootPath);
                });
        }

        [Fact]
        public void TestarConexao()
        {
            using var host = _hostBuilder.Build();
            var serviceProvider = host.Services;
            var dbService = serviceProvider.GetService<IDBService>();
            Assert.NotNull(dbService);
        }
    }
}
