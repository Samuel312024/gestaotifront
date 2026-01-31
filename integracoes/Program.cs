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
using integracoes.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sql =>
        {
            // ⏱ Timeout curto para não travar a API
            sql.CommandTimeout(10);

            // 🔁 Retry CONTROLADO
            sql.EnableRetryOnFailure(
                maxRetryCount: 3,
                maxRetryDelay: TimeSpan.FromSeconds(2),
                errorNumbersToAdd: null
            );
        }
    );
});


// Add services to the container.
var mpToken = builder.Configuration["MercadoPago:AccessToken"];
MercadoPagoConfig.AccessToken = mpToken;
builder.Services.AddSingleton<PaymentClient>();
builder.Services.AddScoped<IConsultaEnderecoService, ConsultaEnderecoService>();
builder.Services.AddScoped<IConsultaPlacaService, ConsultaPlacaService>();
//builder.Services.AddScoped<IPagamentoService, PagamentoService>();

// HttpClient nomeado para Sinesp com Polly (retry exponencial + circuit breaker)
builder.Services.AddHttpClient("SinespClient", client =>
{
    client.Timeout = TimeSpan.FromSeconds(10);
})
.AddPolicyHandler(
    HttpPolicyExtensions
        .HandleTransientHttpError()
        .WaitAndRetryAsync(
            retryCount: 3,
            sleepDurationProvider: retry => TimeSpan.FromMilliseconds(200 * retry)
        )
)
.AddPolicyHandler(
    HttpPolicyExtensions
        .HandleTransientHttpError()
        .CircuitBreakerAsync(
            handledEventsAllowedBeforeBreaking: 3,
            durationOfBreak: TimeSpan.FromSeconds(30)
        )
);

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
