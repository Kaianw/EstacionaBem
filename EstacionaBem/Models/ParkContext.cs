using Microsoft.EntityFrameworkCore;

namespace EstacionaBem.Models
{
    public class ParkContext : DbContext
    {
        public DbSet<PrecoModel> precos { get; set; }

        public DbSet<EstadiaModel> estadias { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite(@"Data Source=C:\Temp\Parking.db");

    }
}
