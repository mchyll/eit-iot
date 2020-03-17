using EitIotService.Models;
using Microsoft.EntityFrameworkCore;

namespace EitIotService.Data
{
    public class SensorDataContext : DbContext
    {
        public SensorDataContext(DbContextOptions<SensorDataContext> options)
            : base(options)
        {
        }

        public DbSet<SensorDevice> SensorDevices { get; set; }
        public DbSet<SensorData> SensorDatas { get; set; }
        public DbSet<Log> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<NbIotDevice>().HasData(new NbIotDevice { DeviceId = "lol" });
            //modelBuilder.Entity<SensorData>().HasData(new SensorData { Id = 1 });
        }
    }
}
