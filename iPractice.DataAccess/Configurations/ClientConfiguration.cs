using iPractice.Scheduling.Domain.SyncAggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iPractice.DataAccess.Configurations
{
    internal class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.ToTable("Clients").HasKey(client => client.Id);
            builder.HasMany(p => p.Psychologists).WithMany(b => b.Clients);
            builder.HasMany(a => a.Appointments).WithOne(a => a.Client).HasForeignKey(a => a.ClientId);
        }
    }
}
