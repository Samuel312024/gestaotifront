using System.ComponentModel.DataAnnotations;

namespace GestaoTI.API.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Nome { get; set; }

        [Required]
        [MaxLength(150)]
        public string Email { get; set; }

        [Required]
        public string SenhaHash { get; set; }

        public string Role { get; set; } = "Usuario";

        public bool Ativo { get; set; } = true;

        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
    }
}
