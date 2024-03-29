using iPractice.Scheduling.Domain.SyncAggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace iPractice.DataAccess.Configurations
{
    internal class PsychologistConfiguration : IEntityTypeConfiguration<Psychologist>
    {
        public void Configure(EntityTypeBuilder<Psychologist> builder)
        {
            builder.ToTable("Psychologists").HasKey(psychologist => psychologist.Id);
            builder.HasMany(p => p.Clients).WithMany(b => b.Psychologists);
            builder.HasMany(a => a.Schedules).WithOne(a => a.Psychologist).HasForeignKey(a => a.PsychologistId);
        }
    }
}
