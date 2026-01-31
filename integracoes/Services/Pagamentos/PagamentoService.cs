//using MercadoPago.Client;
//using MercadoPago.Client.Common;
//using MercadoPago.Client.Payment;
//using MercadoPago.Config;

//namespace integracoes.Services.Pagamentos
//{
//    public class PagamentoService : IPagamentoService
//    {
//        private readonly string _accessToken;

//        public PagamentoService(IConfiguration configuration)
//        {
//            _accessToken = configuration["MercadoPago:AccessToken"];
//            MercadoPagoConfig.AccessToken = _accessToken;
//        }

//        public async Task<PixPaymentResultDto> CriarPagamentoPixAsync(CreatePixRequestDto dto)
//        {
//            var client = new PaymentClient();

//            var request = new PaymentCreateRequest
//            {
//                TransactionAmount = dto.Valor,
//                Description = dto.Descricao,
//                PaymentMethodId = "pix",
//                ExternalReference = dto.ReferenciaExterna,
//                Payer = new PayerRequest  // <--- Correção aqui: PayerRequest (sem Payment)
//                {
//                    Email = dto.EmailPagador,
//                    Identification = new IdentificationRequest  // <--- Correção aqui: IdentificationRequest
//                    {
//                        Type = "CPF",
//                        Number = dto.CpfPagador
//                    }
//                    // Opcional: Adicione mais campos se quiser (ex.: EntityType = "individual", Type = "customer")
//                },
//                DateOfExpiration = DateTime.UtcNow.AddMinutes(30)  // Expira em 30 min
//            };

//            var requestOptions = new RequestOptions();
//            requestOptions.CustomHeaders.Add("X-Idempotency-Key", Guid.NewGuid().ToString());  // Ou Headers.IDEMPOTENCY_KEY

//            var payment = await client.CreateAsync(request, requestOptions);

//            return new PixPaymentResultDto
//            {
//                PaymentId = payment.Id.ToString(),
//                QrCodeTexto = payment.PointOfInteraction?.TransactionData?.QrCode,
//                QrCodeBase64 = payment.PointOfInteraction?.TransactionData?.QrCodeBase64,
//                Status = payment.Status
//            };
//        }
//    }
//}