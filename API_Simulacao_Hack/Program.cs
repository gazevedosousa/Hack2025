using API_Simulacao_Hack.Interfaces.Repositories;
using API_Simulacao_Hack.Interfaces.Services;
using API_Simulacao_Hack.Middleware;
using API_Simulacao_Hack.Models;
using API_Simulacao_Hack.Repositories;
using API_Simulacao_Hack.Services;
using API_Simulacao_Hack.Validators;
using Azure.Messaging.EventHubs.Producer;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure.Amqp;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ISimulacaoService, SimulacaoService>();
builder.Services.AddScoped<IEventHubService, EventHubService>();
builder.Services.AddSingleton<ITelemetriaService, TelemetriaService>();

builder.Services.AddScoped<ISimulacaoRepository, SimulacaoRepository>();

builder.Services.AddScoped<SolicitacaoSimulacaoValidator>();


// contexto de banco de dados
builder.Services.AddDbContext<DbHack>(
              options =>
              {
                  options.UseSqlServer(builder.Configuration.GetConnectionString("DbHack"),
                  providerOptions => providerOptions.EnableRetryOnFailure());
                  options.UseLazyLoadingProxies();
              });

builder.Services.AddDbContext<SimulacaoContext>(
              options =>
              {
                  options.UseSqlite("Data Source=simulacao.db");
              });

// Adiciona o client do Event Hub
builder.Services.AddSingleton(x =>
{
    var connectionString = builder.Configuration.GetConnectionString("EventHub");
    var eventHubName = builder.Configuration.GetValue<string>("EventHubName");
    return new EventHubProducerClient(connectionString, eventHubName);
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAny", builder => builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
    );
});


var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<TelemetriaMiddleware>();

app.UseCors("AllowAny");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
