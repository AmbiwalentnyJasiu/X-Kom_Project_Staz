using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API_Service.Models
{
    public class EventModel
    {
        public EventModel() { }
        public EventModel(string event_name)
        {
            Event_name = event_name;
        }
        public string Event_name { get; set; }

        public int Participants_count { get; set; }
    }

    public class EventConfiguration : IEntityTypeConfiguration<EventModel>
    {
        public void Configure(EntityTypeBuilder<EventModel> builder)
        {
            builder.ToTable("EVENT_NAMES", "dbo");

            builder.HasKey(a => a.Event_name);

            builder.Property(e => e.Event_name).HasColumnType("varchar(20)").IsRequired();
            builder.Property(e => e.Participants_count).HasColumnType("int").HasDefaultValue("0");
        }
    }
}
