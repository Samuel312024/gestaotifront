
namespace integracoes.Controllers
{
    public class RawDataDto
    {
        public int Id { get; internal set; }
        public required string Payload { get; set; }
        public string Cpf { get; set; } = string.Empty;
        public DateTime ReceivedAt { get; set; }
    }
}