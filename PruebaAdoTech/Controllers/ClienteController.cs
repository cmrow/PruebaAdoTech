using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PruebaAdoTech.Data;
using PruebaAdoTech.Modelos;

namespace PruebaAdoTech.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public ClienteController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetClientes()
        {
            IEnumerable<Cliente> lista = _db.Cliente;
            return Ok(lista);

        }

        [HttpGet("{idCliente:int}", Name = "GetCliente")]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetCliente(int? idCliente)
        {
            if (idCliente == null || idCliente == 0)
            {
                return NotFound();
            }
            var obj = _db.Cliente.Find(idCliente);
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
        public IActionResult CrearCliente([FromBody] Cliente cliente)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
                var clienteExistente = _db.Cliente.Where(c => c.NumeroIdentificacion == cliente.NumeroIdentificacion);
            if (clienteExistente.Any())
            {
                ModelState.AddModelError("", "Ya existe el registro");
                return StatusCode(404, ModelState);
            }
            _db.Cliente.Add(cliente);
            _db.SaveChanges();
            return Ok(cliente);

        }

        [HttpPatch("{idCliente:int}", Name = "ActualizarCliente")]
        [ProducesResponseType(typeof(Cliente), 201)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ActualizarCliente(int? idCliente, [FromBody] Cliente cliente)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var clienteExistente = _db.Cliente.Find(idCliente);

            if (clienteExistente != null)
            {
                clienteExistente.NumeroIdentificacion = cliente.NumeroIdentificacion;
                clienteExistente.Nombres = cliente.Nombres;
                clienteExistente.Apellidos = cliente.Apellidos;
                clienteExistente.Genero = cliente.Genero;

                _db.Cliente.Update(clienteExistente);
                _db.SaveChanges();

                return Ok(clienteExistente);

            }
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
            var clienteExistente = _db.Cliente.Find(idCliente);
            if (clienteExistente == null)
            {
                return NotFound();
            }
            _db.Cliente.Remove(clienteExistente);
            _db.SaveChanges();

            return NoContent();


        }






    }
}
