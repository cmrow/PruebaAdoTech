using Microsoft.IdentityModel.Tokens;
using PruebaAdoTech.Data;
using PruebaAdoTech.Modelos;
using PruebaAdoTech.Modelos.DTO;
using PruebaAdoTech.Repositorio.IRepositorio;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using XSystem.Security.Cryptography;

namespace PruebaAdoTech.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly ApplicationDbContext _db;
        private string claveSecerta;
        public UsuarioRepositorio(ApplicationDbContext db, IConfiguration config)
        {
            _db = db;
            claveSecerta = config.GetValue<string>("ApiSettings:Secreta");
        }

        public Usuario GetUsuario(int usuarioId)
        {
            return _db.Usuario.Find(usuarioId);


        }

        public IEnumerable<Usuario> GetUsuarios()
        {
            IEnumerable<Usuario> lista = _db.Usuario.ToList();
            return lista;
        }

        public bool IsUnicoUsuario(string ususario)
        {
            //var usuario = _db.Usuario.Find(ususario);

            var usuario = _db.Usuario.Where(c => c.NombreUsuario == ususario);
            if (usuario == null)
            {
                return true;
            }
            return false;

        }

        public async Task<Usuario> Registro(UsuarioRegistroDto usuarioRegistroDto)
        {
            var PasswordEmcriptado = obtenermd5(usuarioRegistroDto.Password);
            var usuario = new Usuario()
            {
                NombreUsuario = usuarioRegistroDto.NombreUsuario,
                Password = (string)PasswordEmcriptado,
                Nombre = usuarioRegistroDto.Nombre,
                Role = usuarioRegistroDto.Role,
            };
            _db.Usuario.Add(usuario);
            await _db.SaveChangesAsync();
            usuario.Password = (string)PasswordEmcriptado;
            return usuario;
        }

        private object obtenermd5(string password)
        {
            MD5CryptoServiceProvider x = new();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(password);
            data = x.ComputeHash(data);
            string resp = "";
            for (int i = 0; i < data.Length; i++)
                resp += data[i].ToString("x2").ToLower();
            return resp;

        }

        public async Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto)
        {
            var passwordEncriptado = obtenermd5(usuarioLoginDto.Password);
            var usuario = _db.Usuario.Find(usuarioLoginDto);
            if (usuario.Password.ToLower() != passwordEncriptado)
            {
                return new UsuarioLoginRespuestaDto()
                {
                    Token = "",
                    Usuario = null
                };
            }

            var manejadorToken = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(claveSecerta);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.NombreUsuario.ToLower()),
                    new Claim(ClaimTypes.Role, usuario.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = manejadorToken.CreateToken(tokenDescriptor);
            UsuarioLoginRespuestaDto usuarioLoginRespuestaDto = new UsuarioLoginRespuestaDto()
            {
                Token = manejadorToken.WriteToken(token),
                Usuario = usuario
            };
            return usuarioLoginRespuestaDto;
        }


    }

}
