using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using VehicleRental.Application.Abstractions.Clock;
using VehicleRental.Application.Exceptions;
using VehicleRental.Domain.Abstractions;
using VehicleRental.Infrastructure.Outbox;

namespace VehicleRental.Infrastructure
{
    public sealed class ApplicationDbContext : DbContext, IUnitOfWork
    {

        private static readonly JsonSerializerSettings JsonSerializerSettings = new()
        {
            TypeNameHandling = TypeNameHandling.All
        };

        private readonly IDateTimeProvider _dateTimeProvider;

        public ApplicationDbContext(DbContextOptions options, IDateTimeProvider dateTimeProvider) : base(options)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                AddDomainEventsAsOutboxMessages();

                var result = await base.SaveChangesAsync(cancellationToken);
                return result;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new ConcurrencyException("Concurrency exception occurred.", ex);
            }
        }

        private void AddDomainEventsAsOutboxMessages()
        {
            var outboxMessages = ChangeTracker
                 .Entries<IEntity>()
                 .Select(entry => entry.Entity)
                 .SelectMany(entity =>
                 {
                     var domainEvents = entity.GetDomainEvents();
                     entity.ClearDomainEvents();
                     return domainEvents;
                 })
                 .Select(domainEvent => new OutboxMessage(
                     Guid.NewGuid(),
                     _dateTimeProvider.UtcNow,
                     domainEvent.GetType().Name,
                     JsonConvert.SerializeObject(domainEvent, JsonSerializerSettings)
                 )).ToList();


            AddRange(outboxMessages);
        }
    }
}
