using Microsoft.EntityFrameworkCore;
using webapi.Models;

namespace webapi.Servico
{
    public class DbContexto : DbContext
    {
        public DbContexto(DbContextOptions<DbContexto> options) : base(options)
        {
            
        }

        public DbSet<Administrador> Administradores { get; set; }
    }
}