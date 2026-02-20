using MercadoPago.Client.Common;
using MercadoPago.Client.Payment;

namespace integracoes.Services.Pagamentos
{
    internal class PayerRequest : PaymentPayerRequest
    {
        public string Email { get; set; }
        public IdentificationRequest Identification { get; set; }
    }
}