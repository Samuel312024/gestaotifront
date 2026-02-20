using GestaoTI.API.Enums;
using System.ComponentModel.DataAnnotations;

namespace GestaoTI.API.DTOs
{
    public class EditarUsuarioDto
    {
        [Required]
        public string Nome { get; set; }

        [Required]
        public string Email { get; set; }

        public UserRole Role { get; set; }

        public bool Ativo { get; set; }
    }
}
