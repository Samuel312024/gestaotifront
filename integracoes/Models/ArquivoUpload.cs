using System;

namespace integracoes.Models
{
    public class ArquivoUpload
    {
        public int Id { get; set; }

        public string NomeOriginal { get; set; } = null!;
        public string NomeSalvo { get; set; } = null!;
        public string Extensao { get; set; } = null!;
        public long TamanhoBytes { get; set; }
        public string Caminho { get; set; } = null!;

        public string? Status { get; set; }
        public string? TabelaDestino { get; set; }   // 🔴 era string
        public DateTime DataUpload { get; set; }
        public DateTime? DataProcessamento { get; set; } // 🔴 era DateTime
        public string? Erro { get; set; }
    }
}