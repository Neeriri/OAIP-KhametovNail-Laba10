using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Laba10_1
{
    public class BD : DbContext
    {
        private static BD _context;

        public BD()
        {
            try
            {
                Database.EnsureCreated();
            }
            catch (Npgsql.NpgsqlException ex)
            {
                throw new Npgsql.NpgsqlException(
                    $"Ошибка подключения к PostgreSQL: {ex.Message}\n\n" +
                    "Проверьте:\n" +
                    "1. Запущен ли PostgreSQL сервер\n" +
                    "2. Правильность параметров подключения (Host, Database, Username, Password)\n" +
                    "3. Наличие базы данных ResearcherLabDB или прав на её создание", 
                    ex);
            }
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
                // ИЗМЕНИТЕ параметры на ваши:
                // Host - адрес сервера (localhost если локально)
                // Database - имя базы данных
                // Username - имя пользователя PostgreSQL
                // Password - пароль пользователя PostgreSQL
                var connectionString = "Host=localhost;Database=ResearcherLabDB;Username=postgres;Password=REPLACE_WITH_YOUR_PASSWORD";
                
                optionsBuilder.UseNpgsql(connectionString);
            }
        }
    }
}