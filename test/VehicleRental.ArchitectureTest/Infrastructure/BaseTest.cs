using System.Reflection;
using VehicleRental.Application.Abstractions.Messaging;
using VehicleRental.Domain.Abstractions;
using VehicleRental.Infrastructure;

namespace VehicleRental.ArchitectureTest.Infrastructure
{
    public class BaseTest
    {
        protected static readonly Assembly ApplicationAssembly = typeof(IBaseCommand).Assembly;

        protected static readonly Assembly DomainAssembly = typeof(IEntity).Assembly;

        protected static readonly Assembly InfrastructureAssembly = typeof(ApplicationDbContext).Assembly;

        protected static readonly Assembly PresentationAssembly = typeof(Program).Assembly;
    }
}
