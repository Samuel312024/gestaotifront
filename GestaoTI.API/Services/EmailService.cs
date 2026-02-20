using System.Net;
using System.Net.Mail;

namespace GestaoTI.API.Services
{
    public class EmailService
    {
        public async Task EnviarEmailAsync(string destino, string assunto, string mensagem)
        {
            var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(
                    "gestaoti.sistema@gmail.com",
                    "ybku vgrr pbni ngwi"
                ),
                EnableSsl = true
            };

            var mail = new MailMessage
            {
                From = new MailAddress("gestaoti.sistema@gmail.com"),
                Subject = assunto,
                Body = mensagem,
                IsBodyHtml = true
            };

            mail.To.Add(destino);

            await smtp.SendMailAsync(mail);
        }
    }
}
