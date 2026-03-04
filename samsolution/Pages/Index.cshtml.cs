using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MailKit.Net.Smtp;
using MimeKit;

namespace samsolution.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string Nome { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Mensagem { get; set; }
        [BindProperty]
        public bool EmailEnviado { get; set; } = false;


        public void OnGet()
        {
        }

        public IActionResult OnPostEnviar()
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("SAM SOLUTION", "samsolution.150824@gmail.com"));
                message.To.Add(new MailboxAddress("Samuel", "samsolution.150824@gmail.com"));
                message.Subject = "Nova mensagem do site";

                message.Body = new TextPart("plain")
                {
                    Text = $"Nome: {Nome}\nEmail: {Email}\nMensagem: {Mensagem}"
                };

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                    client.Authenticate("samsolution.150824@gmail.com", "btytahapvkhtodfs");
                    client.Send(message);
                    client.Disconnect(true);
                }

                EmailEnviado = true;
                ModelState.Clear();

                return Page();
            }
            catch
            {
                return Page();
            }
        }

    }
}
