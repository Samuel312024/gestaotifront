using GestaoTI.API.Data;
using GestaoTI.API.DTOs;
using GestaoTI.API.Models;
using GestaoTI.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace GestaoTI.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;

        public AuthController(AppDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            if (await _context.Usuarios.AnyAsync(x => x.Email == dto.Email))
                return BadRequest("Usuário já existe");

            var usuario = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha)

            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok("Usuário criado");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (usuario == null ||
                !BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.SenhaHash))
                return Unauthorized("Credenciais inválidas");

            var token = _tokenService.GerarToken(usuario);

            return Ok(new { token });
        }
    }
}
