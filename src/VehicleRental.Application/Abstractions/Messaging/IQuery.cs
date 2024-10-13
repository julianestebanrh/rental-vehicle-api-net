using MediatR;
using VehicleRental.Domain.Abstractions;

namespace VehicleRental.Application.Abstractions.Messaging
{
    public interface IQuery<TResponse> : IRequest<Result<TResponse>>
    {
    }
}
