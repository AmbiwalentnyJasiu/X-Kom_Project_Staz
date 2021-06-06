using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API_Service.Models
{
    /// <summary>
    /// Klasa reprezentujaca pojedynczy wpis w tablicy uczestników
    /// </summary>
    public class ParticipantModel
    {
        public ParticipantModel() { }
        public ParticipantModel(string EVENT_NAME, string F_NAME, string L_NAME, string EMAIL)
        {
            Event_Name = EVENT_NAME;
            F_Name = F_NAME;
            L_Name = L_NAME;
            EMail = EMAIL;
        }

        /// <summary>
        /// SQL Identity dla wpisu
        /// </summary>
        public int Entry_ID { get; set; }

        /// <summary>
        /// Nazwa eventu którego dotyczy wpis
        /// </summary>
        public string Event_Name { get; set; }

        /// <summary>
        /// Imie uczestnika
        /// </summary>
        public string F_Name { get; set; }

        /// <summary>
        /// Nazwisko uczestnika
        /// </summary>
        public string L_Name { get; set; }

        /// <summary>
        /// Adres email uczestnika
        /// </summary>
        public string EMail { get; set; }
    }

    public class ParicipantConfiguration : IEntityTypeConfiguration<ParticipantModel>
    {
        public void Configure(EntityTypeBuilder<ParticipantModel> builder)
        {
            builder.ToTable("PARTICIPANTS", "dbo");

            builder.HasKey(p => p.Entry_ID);

            builder.Property(p => p.Entry_ID).HasColumnType("int").HasDefaultValue();
            builder.Property(p => p.Event_Name).HasColumnType("varchar(20)").IsRequired();
            builder.Property(p => p.F_Name).HasColumnType("varchar(20)").IsRequired();
            builder.Property(p => p.L_Name).HasColumnType("varchar(20)").IsRequired();
            builder.Property(p => p.EMail).HasColumnType("varchar(30)").IsRequired();
        }
    }
}
