using iPractice.Scheduling.Domain.ScheduleAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iPractice.DataAccess.Configurations
{
    internal class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.ToTable("Appointments").HasKey(x => x.Id);
            builder.Property(p => p.Id).ValueGeneratedNever();
            builder.OwnsOne(p => p.TimeSlot, p =>
            {
                p.Property(pp => pp.StartTime)
                .HasColumnName("StartTime");
                p.Property(pp => pp.EndTime)
                .HasColumnName("EndTime");
            });
        }
    }
}
