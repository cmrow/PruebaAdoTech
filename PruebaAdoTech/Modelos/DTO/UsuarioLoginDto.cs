using System.ComponentModel.DataAnnotations;

namespace PruebaAdoTech.Modelos.DTO
{
    public class UsuarioLoginDto
    {
        [Required(ErrorMessage="El usuario es obligatorio")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "El password es obligatorio")]
        public string Password { get; set; }
    }
}
