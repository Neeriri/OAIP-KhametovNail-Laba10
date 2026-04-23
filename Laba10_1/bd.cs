using Laba10_1;
using Microsoft.EntityFrameworkCore;

public class BD : DbContext
{
    private static BD _context;

    static BD()
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

    }
    public BD()
    {
        Database.EnsureCreated();
    }
    public DbSet<Researcher> Researchers { get; set; }
    public DbSet<PasswordResetCode> PasswordResetCodes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = "Host=localhost;Port=5433;Database=ResearcherLabDB;Username=postgres;Password=12345;Timeout=60;Command Timeout=120";
            optionsBuilder.UseNpgsql(connectionString);
        }
    }


    public static BD GetContext()
    {
        if (_context == null)
            _context = new BD();
        return _context;
    }
}