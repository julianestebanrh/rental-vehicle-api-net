using VehicleRental.Domain.Abstractions;

namespace VehicleRental.Domain.Vehicles.Specifications
{
    public class VehicleGetAllSpecification : BaseSpecification<Vehicle, VehicleId>
    {
        public VehicleGetAllSpecification(string sort, int pageIndex, int pageSize, string search) : base(x => string.IsNullOrEmpty(search) || x.Model == new Model(search))
        {
            int skip = pageSize * (pageIndex - 1);
            int take = pageSize;
            ApplyPaging(skip, take);

            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "asc":
                        AddOrderBy(x => x.Model!);
                        break;
                    case "desc":
                        AddOrderByDescending(x => x.Model!);
                        break;
                    default:
                        AddOrderBy(x => x.LastRentedOnUtc!);
                        break;
                }
            }
            else
            {
                AddOrderBy(x => x.LastRentedOnUtc!);
            }
        }
    }
}
