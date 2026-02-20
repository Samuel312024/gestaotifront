using BCrypt.Net;
using GestaoTI.API.Data;
using GestaoTI.API.DTOs;
using GestaoTI.API.Enums;
using GestaoTI.API.Models;
using GestaoTI.API.Services;
using GestaoTI.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

namespace GestaoTI.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;
        private readonly IEmailService _emailService;



        public AuthController(AppDbContext context, TokenService tokenService, IEmailService emailService)
        {
            _context = context;
            _tokenService = tokenService;
            _emailService = emailService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            if (await _context.Usuarios.AnyAsync(x => x.Email == dto.Email))
                return BadRequest("Usuário já existe");

            var usuario = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
                Role = UserRole.User
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok("Usuário criado com sucesso");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser(CreateUserDTO dto)
        {
            if (!Enum.IsDefined(typeof(UserRole), dto.Role))
            {
                return BadRequest("Role inválida");
            }

            if (await _context.Usuarios.AnyAsync(x => x.Email == dto.Email))
                return BadRequest("Usuário já existe");

            if (dto.Role != UserRole.Admin && dto.Role != UserRole.User)
                return BadRequest("Role inválida");

            var usuario = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
                Role = dto.Role
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok("Usuário criado pelo Admin");
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

        [HttpPost("forgot-password")]
        [EnableRateLimiting("forgotPolicy")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (usuario == null)
                return Ok(); // não revela se existe ou não

            var token = Guid.NewGuid().ToString();

            var resetToken = new PasswordResetToken
            {
                Token = token,
                UsuarioId = usuario.Id,
                Expiracao = DateTime.UtcNow.AddMinutes(30)
            };

            _context.PasswordResetTokens.Add(resetToken);
            await _context.SaveChangesAsync();

            // Depois vamos enviar email real
            Console.WriteLine($"TOKEN: {token}");

            return Ok(new { message = "Se o email existir, enviaremos instruções." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var resetToken = await _context.PasswordResetTokens
                .Include(x => x.Usuario)
                .FirstOrDefaultAsync(x => x.Token == dto.Token);

            if (resetToken == null ||
                resetToken.Usado ||
                resetToken.Expiracao < DateTime.UtcNow)
                return BadRequest("Token inválido ou expirado");

            resetToken.Usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.NovaSenha);
            resetToken.Usado = true;

            await _context.SaveChangesAsync();

            return Ok("Senha alterada com sucesso");
        }





    }
}
