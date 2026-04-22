using Microsoft.EntityFrameworkCore;
using Npgsql;

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
        public DbSet<PasswordResetCode> PasswordResetCodes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Подключение к PostgreSQL
                // Замените параметры на ваши: Host, Database, Username, Password
                var connectionString = "Host=localhost;Database=ResearcherLabDB;Username=postgres;Password=your_password";
                optionsBuilder.UseNpgsql(connectionString);
            }
        }
    }
}