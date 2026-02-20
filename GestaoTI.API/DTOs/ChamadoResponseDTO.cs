namespace GestaoTI.API.DTOs
{
    public class ChamadoResponseDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Status { get; set; }
        public DateTime DataAbertura { get; set; }
        public string Tecnico { get; set; }
    }

}
