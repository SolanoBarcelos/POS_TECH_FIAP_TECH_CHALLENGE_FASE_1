// Classe Program para configurar a URL
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Prometheus;
using Npgsql;
using POS_TECH_FASE_UM.Interface;
using POS_TECH_FASE_UM.Service;
using POS_TECH_FASE_UM.Repository;
using System.Data;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configura��o da conex�o com o PostgreSQL
        builder.Services.AddScoped<IDbConnection>(sp =>
        {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            return new NpgsqlConnection(connectionString);
        });

        builder.Services.AddScoped<IContatoRepository, ContatoRepository>();
        builder.Services.AddScoped<IContatoService, ContatoService>();

        // Adiciona suporte a controladores com NewtonsoftJson
        builder.Services.AddControllers().AddNewtonsoftJson();
        builder.Services.AddRazorPages();

        // Configura��o do Swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Configura��o do Prometheus
        builder.Services.UseHttpClientMetrics();

        var app = builder.Build();

        // Configura��o do pipeline de requisi��es HTTP
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // app.UseHttpsRedirection();
        app.UseMetricServer();
        app.UseHttpMetrics();
        app.UseAuthorization();

        // Mapeia os controladores
        app.MapControllers();
        app.MapRazorPages();

        // Configura as portas para escutar somente em ambiente (Docker)
        if (app.Environment.IsProduction())
        {
            app.Urls.Add("http://0.0.0.0:7070");
        }

        app.Run();
    }
}