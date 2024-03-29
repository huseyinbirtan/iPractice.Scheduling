using iPractice.Scheduling.Domain.ScheduleAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iPractice.DataAccess.Configurations
{
    internal class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
    {
        public void Configure(EntityTypeBuilder<Schedule> builder)
        {
            builder.ToTable("Schedules").HasKey(x => x.Id);
            builder.Property(p => p.Id).ValueGeneratedNever();
            builder.HasMany(a => a.Availabilities).WithOne(a => a.Schedule).HasForeignKey(a => a.ScheduleId);
        }
    }
}
