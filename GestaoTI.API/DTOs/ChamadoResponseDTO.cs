namespace GestaoTI.API.DTOs
{
    public class ChamadoResponseDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime DataAbertura { get; set; }
        public string Tecnico { get; set; } = string.Empty;
    }

}
