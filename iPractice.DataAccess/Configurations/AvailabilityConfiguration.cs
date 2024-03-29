using iPractice.Scheduling.Domain.ScheduleAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iPractice.DataAccess.Configurations
{
    internal class AvailabilityConfiguration : IEntityTypeConfiguration<Availability>
    {
        public void Configure(EntityTypeBuilder<Availability> builder)
        {
            builder.ToTable("Availabilities").HasKey(x => x.Id);
            builder.Property(p => p.Id).ValueGeneratedNever();
            builder.HasMany(a => a.Appointments).WithOne(a => a.Availability).HasForeignKey(a => a.AvailabilityId);
            builder.OwnsOne(p => p.AvailabilityTimeSlot, p =>
            {
                p.Property(pp => pp.StartTime)
                .HasColumnName("StartTime");
                p.Property(pp => pp.EndTime)
                .HasColumnName("EndTime");
            });
        }
    }
}
