using SendGrid;
using SendGrid.Helpers.Mail;
using GestaoTI.API.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace GestaoTI.API.Services.Email
{
    public class SendGridEmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public SendGridEmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task EnviarEmailAsync(string destino, string assunto, string mensagem)
        {
            var apiKey = _configuration["SendGrid:ApiKey"];
            var client = new SendGridClient(apiKey);

            var from = new EmailAddress("no-reply@gestaoti.com", "Gestão TI");
            var to = new EmailAddress(destino);

            var msg = MailHelper.CreateSingleEmail(from, to, assunto, "", mensagem);

            await client.SendEmailAsync(msg);
        }
    }
}
