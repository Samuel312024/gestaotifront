using GestaoTI.API.Data;
using GestaoTI.API.Enums;
using GestaoTI.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GestaoTI.API.Controllers
{
    [ApiController]
    [Route("api/chamados")]
    [Authorize]
    public class ChamadoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ChamadoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AbrirChamado(Chamado chamado)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            chamado.UsuarioId = userId;

            _context.Chamados.Add(chamado);
            await _context.SaveChangesAsync();

            return Ok(chamado);
        }

        [HttpGet]
        public IActionResult Listar()
        {
            var chamados = _context.Chamados.ToList();
            return Ok(chamados);
        }

        [HttpGet("meus")]
        public IActionResult MeusChamados()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var chamados = _context.Chamados
                .Where(c => c.UsuarioId == userId)
                .ToList();

            return Ok(chamados);
        }

        [Authorize(Roles = "Admin,Tecnico")]
        [HttpPut("{id}/assumir")]
        public async Task<IActionResult> AssumirChamado(int id)
        {
            var tecnicoId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var chamado = await _context.Chamados.FindAsync(id);

            if (chamado == null)
                return NotFound();

            chamado.TecnicoId = tecnicoId;
            chamado.Status = StatusChamado.EmAtendimento;

            await _context.SaveChangesAsync();

            return Ok(chamado);
        }

        [Authorize(Roles = "Admin,Tecnico")]
        [HttpPut("{id}/fechar")]
        public async Task<IActionResult> FecharChamado(int id)
        {
            var chamado = await _context.Chamados.FindAsync(id);

            if (chamado == null)
                return NotFound();

            chamado.Status = StatusChamado.Fechado;
            chamado.DataFechamento = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(chamado);
        }

    }
}
