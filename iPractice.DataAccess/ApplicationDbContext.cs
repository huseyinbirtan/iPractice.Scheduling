using iPractice.Scheduling.Domain.SyncAggregates;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Threading;
using iPractice.SharedKernel.BaseClasses;
using System;
using iPractice.Scheduling.Domain.ScheduleAggregate;

namespace iPractice.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Availability> Availabilities { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Psychologist> Psychologists { get; set; }
        public DbSet<Client> Clients { get; set; }
        private readonly IMediator Mediator;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IMediator mediator) : base(options)
        {
            Mediator = mediator;
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            // ignore events if no dispatcher provided
            if (Mediator == null) return result;

            var entitiesWithEvents = ChangeTracker
                .Entries()
                .Select(e => e.Entity as BaseEntity<Guid>)
                .Where(e => e?.Events != null && e.Events.Any())
                .ToArray();

            foreach (var entity in entitiesWithEvents)
            {
                var events = entity.Events.ToArray();
                entity.Events.Clear();
                foreach (var domainEvent in events)
                {
                    await Mediator.Publish(domainEvent).ConfigureAwait(false);
                }
            }

            return result;
        }
    }
}
