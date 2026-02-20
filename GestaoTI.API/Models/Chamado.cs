using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GestaoTI.API.Enums;



namespace GestaoTI.API.Models
{
    public class Chamado
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Titulo { get; set; }

        [Required]
        public string Descricao { get; set; }

        //public PrioridadeChamado Prioridade { get; set; }

        public StatusChamado Status { get; set; } = StatusChamado.Aberto;

        public DateTime DataAbertura { get; set; } = DateTime.UtcNow;

        public DateTime? DataFechamento { get; set; }

        [ForeignKey(nameof(Usuario))]
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        [ForeignKey(nameof(Tecnico))]
        public int? TecnicoId { get; set; }
        public Usuario Tecnico { get; set; }
        public string? Prioridade { get; set; }
    }

}
