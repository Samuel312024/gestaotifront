using GestaoTI.API.Data;
using GestaoTI.API.DTOs;
using GestaoTI.API.Enums;
using GestaoTI.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GestaoTI.API.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/usuario")]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuarioController(AppDbContext context)
        {
            _context = context;
        }

        // GET /api/usuario/perfil
        [Authorize]
        [HttpGet("perfil")]
        public IActionResult Perfil()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;

            return Ok(new
            {
                Id = userId,
                Email = email
            });
        }

        // GET /api/usuario
        [HttpGet]
        public async Task<IActionResult> ListarUsuarios()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            return Ok(usuarios);
        }

        [HttpGet("roles")]
        public IActionResult GetRoles()
        {
            var roles = Enum.GetNames(typeof(UserRole));
            return Ok(roles);
        }


        // POST /api/usuario
        [HttpPost]
        public async Task<IActionResult> CriarUsuario([FromBody] CriarUsuarioDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuario = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
                Role = dto.Role
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok(usuario);
        }


        // PUT /api/usuario/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> EditarUsuario(int id, [FromBody] EditarUsuarioDto dto)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound();

            usuario.Nome = dto.Nome;
            usuario.Email = dto.Email;
            usuario.Role = dto.Role;
            usuario.Ativo = dto.Ativo;

            await _context.SaveChangesAsync();

            return Ok(usuario);
        }

    }
}
