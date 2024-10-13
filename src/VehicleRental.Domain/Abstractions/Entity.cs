﻿namespace VehicleRental.Domain.Abstractions
{
    public abstract class Entity<TEntityId> : IEntity
    {
        private readonly List<IDomainEvent> _domainEvents = new();

        protected Entity() { }

        protected Entity(TEntityId id)
        {
            Id = id;
        }

        public TEntityId? Id { get; init; }

        public IReadOnlyList<IDomainEvent> GetDomainEvents()
        {
            return _domainEvents.ToList();
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        public void RaiseDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }
    }
}