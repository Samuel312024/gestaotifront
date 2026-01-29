//using System.ComponentModel.DataAnnotations;

//namespace integracoes.Models
//{
//    public class ConsultaPlaca
//    {
//        [Key]  // ← Isso resolve o erro!
//        public int Id { get; set; }  // Ou use Guid Id { get; set; }

//        public string Placa { get; set; } = string.Empty;
//        public string Modelo { get; set; } = string.Empty;
//        public string Cor { get; set; } = string.Empty;
//        public string Status { get; set; } = string.Empty;
//        public DateTime DataConsulta { get; set; } = DateTime.UtcNow;
//    }
//}