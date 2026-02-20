public class RawDataDto
{
    public int Id { get; internal set; }
    public required string Payload { get; set; }
    public DateTime ReceivedAt { get; set; }
}