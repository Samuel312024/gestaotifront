
namespace integracoes.Controllers
{
    public class RawDataDto
    {
        public int Id { get; internal set; }
        public required string Payload { get; set; }
        public required string Cpf { get; set; }
        public DateTime ReceivedAt { get; set; }
    }
}