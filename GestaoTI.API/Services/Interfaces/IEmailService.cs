namespace GestaoTI.API.Services.Interfaces
{
    public interface IEmailService
    {
        Task EnviarEmailAsync(string destino, string assunto, string mensagem);
    }
}
