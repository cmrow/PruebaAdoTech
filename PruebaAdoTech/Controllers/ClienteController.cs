using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PruebaAdoTech.Data;
using PruebaAdoTech.Modelos;
using PruebaAdoTech.Repositorio;
using PruebaAdoTech.Repositorio.IRepositorio;
using XAct;

namespace PruebaAdoTech.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IRepository<Cliente> _db;
        public ClienteController(IRepository<Cliente> db)
        {
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetClientes()
        {
            IEnumerable<Cliente> list = _db.Get();
            return Ok(list);
        }

        [HttpGet("{idCliente:int}", Name = "GetCliente")]
        [ProducesResponseType(404)]
        public IActionResult GetCliente(int? idCliente)
        {
            if (idCliente == null || idCliente == 0)
            {
                return NotFound();
            }
            var obj = _db.Get((int)idCliente);
            if (obj == null)
            {
                return NotFound();

            }
            return Ok(obj);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Cliente), 201)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateCliente([FromBody] Cliente cliente)
        {

            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            IEnumerable<Cliente> list = _db.Get();
            if (list != null)
            {
                var clienteExistente = list.Where(b => b.NumeroIdentificacion == cliente.NumeroIdentificacion)
                        .FirstOrDefault();
                if (clienteExistente != null)
                {
                    ModelState.AddModelError("", "Ya existe el registro");
                    return StatusCode(404, ModelState);
                }
            }
            _db.Add(cliente);
            _db.Save();
            return Ok(cliente);
        }
        [HttpPatch("{idCliente:int}", Name = "ActualizarCliente")]
        [ProducesResponseType(typeof(Cliente), 201)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ActualizarCliente(int? idCliente, [FromBody] Cliente cliente)
        {
            if (!ModelState.IsValid || idCliente == null) return BadRequest(ModelState);
            var clienteUpdate = _db.Get(Convert.ToInt32(idCliente));
            if (clienteUpdate == null)
            {
                ModelState.AddModelError("", "Cliente no registrado");
                return BadRequest(ModelState);
            }
            clienteUpdate.NumeroIdentificacion = cliente.NumeroIdentificacion;
            clienteUpdate.Nombres = cliente.Nombres;
            clienteUpdate.Apellidos = cliente.Apellidos;
            clienteUpdate.Genero = cliente.Genero;
            _db.Update(clienteUpdate);
            _db.Save();
            return NoContent();

        }
        [HttpDelete("{idCliente:int}", Name = "ActualizarCliente")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult BorrarCliente(int? idCliente)
        {

            if (idCliente == null || idCliente == 0)
            {
                return NotFound();
            }
            var clienteExistente = _db.Get((int)idCliente);
            if (clienteExistente == null)
            {
                return NotFound();
            }
            _db.Delete((int)idCliente);
            _db.Save();

            return NoContent();


        }
    }
} 
