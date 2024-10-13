using AutoMapper;
using VehicleRental.Application.Users.GetAllUser;
using VehicleRental.Domain.Users;

namespace VehicleRental.Application.Mappings
{
    internal sealed class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, GetAllUserQueryDto>()
                .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id.Value))
                .ForMember(dest => dest.FirstName, src => src.MapFrom(x => x.FirstName.Value))
                .ForMember(dest => dest.LastName, src => src.MapFrom(x => x.LastName.Value))
                .ForMember(dest => dest.Email, src => src.MapFrom(x => x.Email.Value))
                .ForMember(dest => dest.Roles, src => src.MapFrom(x => x.Roles.Select(x => x.Name).ToList()))
                .ForMember(dest => dest.Permissions, src => src.MapFrom(x => x.Roles.SelectMany(x => x.Permissions).Select(x => x.Name.Value).ToHashSet()));

        }
    }
}
