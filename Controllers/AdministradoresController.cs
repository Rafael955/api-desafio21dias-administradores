using System.Linq;
using System.Threading.Tasks;
using EntityFrameworkPaginateCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Models;
using webapi.Servico;

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
        public async Task<IActionResult> Index(int page = 1)
        {
            return StatusCode(200, await _context.Administradores.OrderBy(x => x.Id).PaginateAsync(page, QUANTIDADE_POR_PAGINA));
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Administrador administrador)
        {
            _context.Add(administrador);
            await _context.SaveChangesAsync();
            return Ok(administrador);
        }

        // POST: Administradores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut("atualizar-administrador/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Administrador administrador)
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
                if (!MaterialExists(administrador.Id))
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var administrador = await _context.Administradores.FindAsync(id);
            
            if(administrador == null)
                return NotFound("administrador não encontrado");

            _context.Administradores.Remove(administrador);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool MaterialExists(int id)
        {
            return _context.Administradores.Any(e => e.Id == id);
        }
    }
}
