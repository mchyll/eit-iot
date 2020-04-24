using EitIotService.Models;
using Microsoft.EntityFrameworkCore;

namespace EitIotService.Data
{
    /// <summary>
    /// Represents the database for querying and saving entities.
    /// </summary>
    public class SensorDataContext : DbContext
    {
        public SensorDataContext(DbContextOptions<SensorDataContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Collection with all sensor device entries from the database.
        /// </summary>
        public DbSet<SensorDevice> SensorDevices { get; set; }

        /// <summary>
        /// Collection with all datapoints in the database from sensor devices.
        /// </summary>
        public DbSet<SensorData> SensorDatas { get; set; }

        /// <summary>
        /// Collection with debugging logs used during testing and development.
        /// </summary>
        public DbSet<Log> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<NbIotDevice>().HasData(new NbIotDevice { DeviceId = "lol" });
            //modelBuilder.Entity<SensorData>().HasData(new SensorData { Id = 1 });
        }
    }
}
