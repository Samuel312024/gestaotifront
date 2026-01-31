using System;
using System.Text.Json;
using System.Threading.Tasks;
using integracoes.Data;
using Microsoft.AspNetCore.Mvc;


namespace integracoes.Controllers
{
    [ApiController]
    [Route("api/armazenar")]
    public class GuardarController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GuardarController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("guardar")]
        public async Task<IActionResult> GuardarDados([FromBody] object dados)
        {
            var entry = new RawDataDto
            {
                Payload = System.Text.Json.JsonSerializer.Serialize(dados),
                ReceivedAt = DateTime.UtcNow
            };

            _context.RawDatas.Add(entry);
            await _context.SaveChangesAsync();

            return Ok(new { Mensagem = "Dados guardados com sucesso", entry.Id });
        }
    }
}
