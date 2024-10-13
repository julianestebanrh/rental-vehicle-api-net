using AutoMapper;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using VehicleRental.Application.Abstractions.Messaging;
using VehicleRental.Application.Abstractions.Pagination;
using VehicleRental.Domain.Abstractions;
using VehicleRental.Domain.Users;

namespace VehicleRental.Application.Users.GetAllUser
{
    internal sealed class GetAllUserQueryHandler : IQueryHandler<GetAllUserQuery, PagedResult<GetAllUserQueryDto>>
    {
        private readonly IMapper _mapper;
        private readonly IPaginationRepository<User, UserId> _paginationRepository;

        public GetAllUserQueryHandler(IPaginationRepository<User, UserId> paginationRepository, IMapper mapper)
        {
            _paginationRepository = paginationRepository;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<GetAllUserQueryDto>>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            var builder = PredicateBuilder.New<User>(true);

            if (!string.IsNullOrEmpty(request.Search))
            {
                builder = builder.Or(p => p.FirstName == new FirstName(request.Search));
                builder = builder.Or(p => p.Email == new Email(request.Search));
            }

            var userQuery = _paginationRepository.GetAllAsync(
                builder,
                p => p.Include(x => x.Roles!).ThenInclude(y => y.Permissions!),
                request.PageIndex,
                request.PageSize,
                request.OrderBy!,
                request.OrderAsc
            );

            var users = await PagedResult<GetAllUserQueryDto>.CreateAsync(userQuery, request.PageIndex, request.PageSize, _mapper);

            return users;
        }
    }
}
