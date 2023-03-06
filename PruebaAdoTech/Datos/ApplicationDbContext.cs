using Microsoft.EntityFrameworkCore;
using PruebaAdoTech.Modelos;

namespace PruebaAdoTech.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
    }
}
