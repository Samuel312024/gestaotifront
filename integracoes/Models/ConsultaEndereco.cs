using System.ComponentModel.DataAnnotations;

namespace integracoes.Models
{
    public class ConsultaEndereco
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(8)]
        public string Cep { get; set; } = string.Empty;

        public string Logradouro { get; set; } = string.Empty;
        public string Complemento { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Localidade { get; set; } = string.Empty;
        public string Uf { get; set; } = string.Empty;
        public string Ibge { get; set; } = string.Empty;
        public string Gia { get; set; } = string.Empty;
        public string Ddd { get; set; } = string.Empty;
        public string Siafi { get; set; } = string.Empty;

        public DateTime DataConsulta { get; set; } = DateTime.UtcNow;
        public string Origem { get; set; } = "ViaCEP";  // Para rastrear fonte
    }
}