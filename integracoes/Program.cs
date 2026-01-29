using integracoes.Data;
using integracoes.Services.Consultas;
using integracoes.Services.Pagamentos;
using MercadoPago.Client.Payment;
using MercadoPago.Config;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Extensions.Http;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Polly.Registry;

var builder = WebApplication.CreateBuilder(args);

// DbContext com retry configurado explicitamente
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 6,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: new[] { 4060, 10928, 10929, 40197, 40501, 40613 }
        )
    ));
    
// Add services to the container.
var mpToken = builder.Configuration["MercadoPago:AccessToken"];
MercadoPagoConfig.AccessToken = mpToken;
builder.Services.AddSingleton<PaymentClient>();
builder.Services.AddScoped<IConsultaEnderecoService, ConsultaEnderecoService>();
builder.Services.AddScoped<IConsultaPlacaService, ConsultaPlacaService>();
builder.Services.AddScoped<IPagamentoService, PagamentoService>();

// HttpClient nomeado para Sinesp com Polly (retry exponencial + circuit breaker)
builder.Services.AddHttpClient("SinespClient", client =>
{
    client.Timeout = TimeSpan.FromSeconds(30);
})
.AddPolicyHandler(HttpPolicyExtensions
    .HandleTransientHttpError()
    .CircuitBreakerAsync(
        handledEventsAllowedBeforeBreaking: 3,
        durationOfBreak: TimeSpan.FromSeconds(30)
    ));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
