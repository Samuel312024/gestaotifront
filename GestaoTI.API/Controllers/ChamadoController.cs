using GestaoTI.API.Data;
using GestaoTI.API.DTOs;
using GestaoTI.API.Enums;
using GestaoTI.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GestaoTI.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/chamados")]
    public class ChamadoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ChamadoController(AppDbContext context)
        {
            _context = context;
        }

        //[HttpPost]
        //public async Task<IActionResult> AbrirChamado(Chamado chamado)
        //{
        //    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        //    if (!int.TryParse(userIdClaim, out int userId))
        //    {
        //        return Unauthorized("Token inválido");
        //    }


        //    chamado.UsuarioId = userId;

        //    _context.Chamados.Add(chamado);
        //    await _context.SaveChangesAsync();

        //    return Ok(chamado);
        //}

        [HttpGet]
        public IActionResult Listar()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("Token inválido");
            }


            IQueryable<Chamado> query = _context.Chamados;

            if (role == "User")
            {
                query = query.Where(c => c.UsuarioId == userId);
            }

            var chamados = query.ToList();

            return Ok(chamados);
        }



        [HttpGet("meus")]
        public IActionResult MeusChamados()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("Token inválido");
            }


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

        [Authorize(Roles = "Admin")]
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

        [HttpGet("stats")]
        public IActionResult Stats()
        {
            var total = _context.Chamados.Count();

            var pendentes = _context.Chamados
                .Count(c => c.Status == StatusChamado.Aberto);

            var emAtendimento = _context.Chamados
                .Count(c => c.Status == StatusChamado.EmAtendimento);

            var fechados = _context.Chamados
                .Count(c => c.Status == StatusChamado.Fechado);

            return Ok(new
            {
                total,
                pendentes,
                emAtendimento,
                fechados
            });
        }

        [HttpPost]
        public async Task<IActionResult> AbrirChamado(AbrirChamadoDTO dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("Token inválido");
            }


            var chamado = new Chamado
            {
                Titulo = dto.Titulo,
                Descricao = dto.Descricao,
                UsuarioId = userId,
                Prioridade = string.IsNullOrWhiteSpace(dto.Prioridade)
                     ? "Baixa"
                     : dto.Prioridade,
                Status = StatusChamado.Aberto,
                DataAbertura = DateTime.UtcNow
            };

            _context.Chamados.Add(chamado);
            await _context.SaveChangesAsync();

            return Ok(chamado);
        }

    }
}
