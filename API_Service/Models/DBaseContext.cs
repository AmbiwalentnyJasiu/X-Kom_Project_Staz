using Microsoft.EntityFrameworkCore;

namespace API_Service.Models
{
    public class DBaseContext : DbContext
    {
        public DBaseContext(DbContextOptions<DBaseContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder
                .ApplyConfiguration(new ParicipantConfiguration());

            modelBuilder
                .ApplyConfiguration(new EventConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<EventModel> EVENT_NAMES { get; set; }

        public DbSet<ParticipantModel> PARTICIPANTS { get; set; }
    }
}
