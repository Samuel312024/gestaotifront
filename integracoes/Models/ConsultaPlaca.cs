using System.ComponentModel.DataAnnotations;

 namespace integracoes.Models
{
    public class ConsultaPlaca
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(7)]
        public string Placa { get; set; } = string.Empty;

        public string? Chassi { get; set; } 
        public string? Marca { get; set; } 
        public string? Modelo { get; set; } 
        public string? Ano { get; set; } 
        public string? Cor { get; set; } 
        public string? Situacao { get; set; } 
        public string? Cidade { get; set; } 
        public string? Uf { get; set; } 
        public DateTime? DataUltimaConsulta { get; set; }

        public DateTime DataConsulta { get; set; } = DateTime.UtcNow;
        public string Origem { get; set; } = "Sinesp";  // ou outra fonte
    }
}