using System;
using System.Linq;
using System.Threading.Tasks;

namespace integracoes.Tools
{
    internal class Program
    {
        static async Task<int> Main(string[] args)
        {
            // Prioridade: arg > env MercadoPago__AccessToken > env MERCADOPAGO_ACCESS_TOKEN > prompt
            string token = args.FirstOrDefault()
                ?? Environment.GetEnvironmentVariable("MercadoPago__AccessToken")
                ?? Environment.GetEnvironmentVariable("MERCADOPAGO_ACCESS_TOKEN");

            if (string.IsNullOrWhiteSpace(token))
            {
                Console.Write("Insira o Access Token do MercadoPago (TEST-/PROD-): ");
                token = Console.ReadLine()?.Trim() ?? string.Empty;
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                Console.WriteLine("Access Token não informado. Encerrando.");
                return 1;
            }

            try
            {
                Console.WriteLine("Iniciando requisição bruta ao MercadoPago...");
                await TestMercadoPagoRawRequest.TestAsync(token);
                Console.WriteLine("Teste finalizado.");
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao executar o teste:");
                Console.WriteLine(ex);
                return 2;
            }
        }
    }

    // Adicione esta classe para corrigir o erro CS1061
    internal static class TestMercadoPagoRawRequest
    {
        public static async Task TestAsync(string token)
        {
            // Implemente a lógica real aqui conforme necessário
            await Task.Delay(100); // Simulação de chamada assíncrona
            Console.WriteLine($"Token recebido: {token}");
        }
    }
}