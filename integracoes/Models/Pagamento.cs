namespace integracoes.Models
{
    public class Pagamento
    {
        public int Id { get; set; }
        public decimal Valor { get; set; }
        public string PaymentId { get; set; } = string.Empty;  // ID do Mercado Pago
        public string Status { get; set; } = string.Empty;
        public string QrCodeTexto { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    }
}