using System.ComponentModel.DataAnnotations;

namespace integracoes.Models
{
    public class RawData
    {
        public int Id { get; set; }

        [Required]
        public string? Payload { get; set; } = string.Empty;

        [Required]
        [MaxLength(11)]
        public string? Cpf { get; set; } = string.Empty;

        public DateTime? ReceivedAt { get; set; }
    }
}
