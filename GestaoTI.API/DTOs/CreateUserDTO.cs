using GestaoTI.API.Enums;

namespace GestaoTI.API.DTOs
{
    public class CreateUserDTO
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public UserRole Role { get; set; }
    }


}
