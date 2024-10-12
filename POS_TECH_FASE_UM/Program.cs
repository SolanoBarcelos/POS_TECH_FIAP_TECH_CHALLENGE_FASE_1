using Npgsql;
using POS_TECH_FASE_UM.Interface;
using POS_TECH_FASE_UM.Service;
using POS_TECH_FASE_UM.Repository;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Configura��o da conex�o com o PostgreSQL
builder.Services.AddScoped<IDbConnection>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    return new NpgsqlConnection(connectionString);
});

// Registro de reposit�rios e servi�os
builder.Services.AddScoped<IContatoRepository, ContatoRepository>();
builder.Services.AddScoped<IContatoService, ContatoService>();

// Adiciona suporte a controladores
builder.Services.AddControllers();
builder.Services.AddRazorPages();

// Configura��o do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configura��o do pipeline de requisi��es HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection(); // Descomente se precisar de HTTPS
app.UseAuthorization();

// Mapeia os controladores
app.MapControllers();
app.MapRazorPages();

// Configura as portas para escutar
//app.Urls.Clear();
//app.Urls.Add("http://0.0.0.0:5000");

// Executa a aplica��o
app.Run();
