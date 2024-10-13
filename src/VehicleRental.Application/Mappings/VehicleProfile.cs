using AutoMapper;
using VehicleRental.Application.Vehicles.GetAllVehicles;
using VehicleRental.Domain.Vehicles;

namespace VehicleRental.Application.Mappings
{
    internal sealed class VehicleProfile : Profile
    {
        public VehicleProfile()
        {
            CreateMap<Vehicle, GetAllVehicleQueryDto>()
                .ForMember(dest => dest.Model, src => src.MapFrom(x => x.Model!.Value))
                .ForMember(dest => dest.Vin, src => src.MapFrom(x => x.Vin!.Value))
                .ForMember(dest => dest.Price, src => src.MapFrom(x => x.Price!.Amount))
                .ForMember(dest => dest.PriceCurrency, src => src.MapFrom(x => x.Price!.Currency.Code))
                .ForMember(dest => dest.MaintenanceFee, src => src.MapFrom(x => x.MaintenanceFee!.Amount))
                .ForMember(dest => dest.MaintenanceFeeCurrency, src => src.MapFrom(x => x.MaintenanceFee!.Currency.Code))
                .ForMember(dest => dest.Amenities, src => src.MapFrom(x => x.Amenities.Select(a => a.ToString()).ToList()))
                .ForMember(dest => dest.LastRentedOnUtc, src => src.MapFrom(x => x.LastRentedOnUtc))
                .ForMember(dest => dest.Address, src => src.MapFrom(x => x.Address!));

            CreateMap<Address, AddressDto>().ReverseMap();
        }
    }
}
