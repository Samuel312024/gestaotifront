using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using GestaoTI.API.Services.Interfaces;
using GestaoTI.API.Models;

namespace GestaoTI.API.Services.Email
{
    public class GmailEmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public GmailEmailService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task EnviarEmailAsync(string destino, string assunto, string mensagem)
        {
            var smtp = new SmtpClient(_settings.Host, _settings.Port)
            {
                Credentials = new NetworkCredential(
                    _settings.Email,
                    _settings.Password
                ),
                EnableSsl = true
            };

            var mail = new MailMessage
            {
                From = new MailAddress(_settings.Email),
                Subject = assunto,
                Body = mensagem,
                IsBodyHtml = true
            };

            mail.To.Add(destino);

            await smtp.SendMailAsync(mail);
        }
    }
}
