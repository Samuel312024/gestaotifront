namespace GestaoTI.API.Models
{
    public class PasswordResetToken
    {
        public int Id { get; set; }

        public string Token { get; set; }

        public DateTime Expiracao { get; set; }

        public bool Usado { get; set; } = false;

        public int UsuarioId { get; set; }

        public Usuario Usuario { get; set; }
    }

}
