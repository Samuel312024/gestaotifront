using GestaoTI.API.Data;
using GestaoTI.API.Models;
using GestaoTI.API.Services;
using GestaoTI.API.Services.Email;
using GestaoTI.API.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// 🔹 BANCO DE DADOS
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
            )
        };
    });
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});



builder.Services.AddScoped<TokenService>();

// 🔹 CONTROLLERS + SWAGGER
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<IEmailService, GmailEmailService>();
builder.Services.Configure<EmailSettings>(
builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, SendGridEmailService>();
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("forgotPolicy", config =>
    {
        config.Window = TimeSpan.FromMinutes(1);
        config.PermitLimit = 3;
        config.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
        config.QueueLimit = 0;
    });
});
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });


var app = builder.Build();

// 🔹 PIPELINE
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

}
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();
app.UseCors();



app.MapControllers();
app.Run();
