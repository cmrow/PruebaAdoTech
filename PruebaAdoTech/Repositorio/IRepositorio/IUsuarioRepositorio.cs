using PruebaAdoTech.Modelos;
using PruebaAdoTech.Modelos.DTO;

namespace PruebaAdoTech.Repositorio.IRepositorio
{
    public interface IUsuarioRepositorio
    {
        IEnumerable<Usuario> GetUsuarios();

        Usuario GetUsuario(int usuarioId);

        bool IsUnicoUsuario(string ususario);

        Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto);
        Task<Usuario> Registro(UsuarioRegistroDto usuarioRegistroDto);



    }
}
