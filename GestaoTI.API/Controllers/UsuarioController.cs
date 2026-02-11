using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestaoTI.API.Controllers
{
    [ApiController]
    [Route("api/usuario")]
    public class UsuarioController : ControllerBase
    {
        [Authorize]
        [HttpGet("perfil")]
        public IActionResult Perfil()
        {
            return Ok("Você está autenticado 🔐");
        }
    }
}
