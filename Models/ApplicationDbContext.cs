using Microsoft.EntityFrameworkCore;

namespace CRUD_AspNet.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public DbSet<Product> Products { get; set; }
    }
}
