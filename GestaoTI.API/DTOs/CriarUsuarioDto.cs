using GestaoTI.API.Enums;
using System.ComponentModel.DataAnnotations;

namespace GestaoTI.API.DTOs
{
    public class CriarUsuarioDto
    {
        [Required]
        public string Nome { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Senha { get; set; }

        public UserRole Role { get; set; } = UserRole.User;
    }
}
