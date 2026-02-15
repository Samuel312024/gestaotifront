using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GestaoTI.API.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/usuario")]
    public class UsuarioController : ControllerBase
    {
        [Authorize]
        [HttpGet("perfil")]
        public IActionResult Perfil()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            return Ok(new
            {
                Id = userId,
                Email = email
            });
        }
    }
}
