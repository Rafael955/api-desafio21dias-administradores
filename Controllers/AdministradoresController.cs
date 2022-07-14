using System.Linq;
using System.Threading.Tasks;
using EntityFrameworkPaginateCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Models;
using webapi.Servico;
using webapi_administradores.ModelViews;

namespace webapi.Controllers
{
    [ApiController]
    [Route("administradores/api")]
    public class AdministradoresController : Controller
    {
        private readonly DbContexto _context;

        private const int QUANTIDADE_POR_PAGINA = 3;

        public AdministradoresController(DbContexto context)
        {
            _context = context;
        }

        // GET: Administradores
        [HttpGet("listar-administradores")]
        public async Task<IActionResult> Index([FromQuery]int page = 1)
        {
            return StatusCode(200, await _context.Administradores.Select(x => new
            {
                Id = x.Id,
                Nome = x.Nome,
                Email =x.Email
            }).OrderBy(x => x.Id).PaginateAsync(page, QUANTIDADE_POR_PAGINA));
        }

         // POST: Administradores/Login
        [HttpPost("administrador/login")]
        public async Task<IActionResult> Login([FromBody] AdmLoginView admin)
        {
            if (string.IsNullOrEmpty(admin.Email) || string.IsNullOrEmpty(admin.Senha))
            {
                return StatusCode(400, new
                {
                    Mensagem = "É obrigatório passar e-mail e senha"
                });
            }

            var administrador = await _context.Administradores
                .FirstOrDefaultAsync(a => a.Email == admin.Email && a.Senha == admin.Senha);

            if (administrador != null)
            {
                return StatusCode(200, new
                {
                    Id = administrador.Id,
                    Nome = administrador.Nome,
                    Email = administrador.Email
                });
            }

            return StatusCode(401, new
            {
                Mensagem = "Usuário ou Senha não permitido"
            });
        }

        // GET: Administradores/Details/5
        [HttpGet("detalhes-administrador/{id:int?}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var administrador = await _context.Administradores
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (administrador == null)
            {
                return NotFound();
            }

            return Ok(administrador);
        }

        // POST: Administradores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("cadastrar-administrador")]
        public async Task<IActionResult> Create([FromBody] Administrador administrador)
        {
            _context.Add(administrador);
            await _context.SaveChangesAsync();
            
            return StatusCode(201, new 
            {
                Id = administrador.Id,
                Nome = administrador.Nome,
                Email = administrador.Email
            });
        }

        // POST: Administradores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut("atualizar-administrador/{id:int}")]
        public async Task<IActionResult> Edit(int id, [FromBody] Administrador administrador)
        {
            if (id != administrador.Id)
            {
                return NotFound();
            }

            try
            {
                _context.Update(administrador);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdministradorExists(administrador.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            
            return Ok(administrador);
        }

        // POST: Administradores/Delete/5
        [HttpDelete("remover-administrador/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var administrador = await _context.Administradores.FindAsync(id);
            
            if(administrador == null)
                return NotFound("administrador não encontrado");

            _context.Administradores.Remove(administrador);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool AdministradorExists(int id)
        {
            return _context.Administradores.Any(e => e.Id == id);
        }
    }
}
