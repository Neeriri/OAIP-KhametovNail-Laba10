using Microsoft.EntityFrameworkCore;

namespace Laba10_1
{
    public class BD : DbContext
    {
        private static BD _context;

        public BD()
        {
            Database.EnsureCreated();
        }

        public static BD GetContext()
        {
            if (_context == null)
                _context = new BD();
            return _context;
        }

        public DbSet<Researcher> Researchers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=ResearcherLabDB;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }
    }
}