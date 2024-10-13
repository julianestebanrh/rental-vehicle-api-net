using VehicleRental.Domain.Abstractions;
using VehicleRental.Domain.Rentals.Events;
using VehicleRental.Domain.Shared;
using VehicleRental.Domain.Users;
using VehicleRental.Domain.Vehicles;

namespace VehicleRental.Domain.Rentals
{
    public sealed class Rental : Entity<RentalId>
    {

        private Rental() { }

        private Rental(
            RentalId id,
            VehicleId vehicleId,
            UserId userId,
            DateRange duration,
            Money priceForPeriod,
            Money maintenanceFee,
            Money amenitiesUpCharge,
            Money totalPrice,
            RentalStatus status,
            DateTime createdOnUtc) : base(id)
        {
            VehicleId = vehicleId;
            UserId = userId;
            PriceForPeriod = priceForPeriod;
            MaintenanceFee = maintenanceFee;
            AmenitiesUpCharge = amenitiesUpCharge;
            TotalPrice = totalPrice;
            Status = status;
            Duration = duration;
            CreatedOnUtc = createdOnUtc;
        }

        public VehicleId? VehicleId { get; private set; }
        public UserId? UserId { get; private set; }
        public Money? PriceForPeriod { get; private set; }
        public Money? MaintenanceFee { get; private set; }
        public Money? AmenitiesUpCharge { get; private set; }
        public Money? TotalPrice { get; private set; }
        public RentalStatus Status { get; private set; }
        public DateRange? Duration { get; private set; }
        public DateTime CreatedOnUtc { get; private set; }
        public DateTime? ConfirmedOnUtc { get; private set; }
        public DateTime? RejectedOnUtc { get; private set; }
        public DateTime? CompletedOnUtc { get; private set; }
        public DateTime? CancelledOnUtc { get; private set; }

        public static Rental Reserve(
            Vehicle vehicle,
            UserId userId,
            DateRange duration,
            DateTime createdOnUtc,
            PricingService pricingService)
        {
            var pricingDetail = pricingService.CalculatePrice(vehicle, duration);

            var rental = new Rental(
                RentalId.New(),
                vehicle.Id!,
                userId,
                duration,
                pricingDetail.PriceForPeriod,
                pricingDetail.MaintenanceFee,
                pricingDetail.AmenitiesUpCharge,
                pricingDetail.TotalPrice,
                RentalStatus.Reserved,
                createdOnUtc);

            rental.RaiseDomainEvent(new RentalReservedDomainEvent(rental.Id!));
            vehicle.LastRentedOnUtc = createdOnUtc;
            return rental;
        }

        public Result Confirm(DateTime utcNow)
        {
            if (Status != RentalStatus.Reserved)
            {
                return Result.Failure(RentalErrors.NotReserved);
            }

            Status = RentalStatus.Confirmed;
            ConfirmedOnUtc = utcNow;

            RaiseDomainEvent(new RentalConfirmedDomainEvent(Id!));

            return Result.Success();
        }

        public Result Reject(DateTime utcNow)
        {
            if (Status != RentalStatus.Reserved)
            {
                return Result.Failure(RentalErrors.NotReserved);
            }

            Status = RentalStatus.Rejected;
            RejectedOnUtc = utcNow;

            RaiseDomainEvent(new RentalRejectedDomainEvent(Id!));

            return Result.Success();
        }

        public Result Complete(DateTime utcNow)
        {
            if (Status != RentalStatus.Confirmed)
            {
                return Result.Failure(RentalErrors.NotConfirmed);
            }

            Status = RentalStatus.Completed;
            CompletedOnUtc = utcNow;

            RaiseDomainEvent(new RentalCompletedDomainEvent(Id!));

            return Result.Success();
        }

        public Result Cancel(DateTime utcNow)
        {
            if (Status != RentalStatus.Confirmed)
            {
                return Result.Failure(RentalErrors.NotConfirmed);
            }

            var currentDate = DateOnly.FromDateTime(utcNow);

            if (currentDate > Duration!.Start)
            {
                return Result.Failure(RentalErrors.AlreadyStarted);
            }

            Status = RentalStatus.Cancelled;
            CancelledOnUtc = utcNow;

            RaiseDomainEvent(new RentalCancelledDomainEvent(Id!));

            return Result.Success();
        }

    }
}
