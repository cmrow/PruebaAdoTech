using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PruebaAdoTech.Data;
using PruebaAdoTech.Modelos;
using PruebaAdoTech.Modelos.DTO;
using PruebaAdoTech.Repositorio.IRepositorio;

namespace PruebaAdoTech.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        //private readonly IUsuarioRepositorio _ctRepo;
        private readonly ApplicationDbContext _db;
        private string claveSecerta;

        public UsuarioController(ApplicationDbContext db, IConfiguration config)
        {
            _db = db;
            claveSecerta = config.GetValue<string>("ApiSettings:Secreta");
        }

        [HttpGet]
        [ProducesResponseType(404)]
        public IActionResult GetUsuarios()
        {
            IEnumerable<Usuario> listaUsuarios = _db.Usuario;
            return Ok(listaUsuarios);
        }

        [HttpGet("{usuarioId:int}", Name = "GetUsuario")]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetUsuario(int? usuarioId)
        {
            if (usuarioId == null || usuarioId == 0)
            {
                return NotFound();
            }
            var obj = _db.Usuario.Find(usuarioId);
            if (obj == null)
            {
                return NotFound();

            }
            return Ok(obj);

        }


    }
}
