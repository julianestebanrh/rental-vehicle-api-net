using VehicleRental.Domain.Abstractions;
using VehicleRental.Domain.Roles;
using VehicleRental.Domain.Users.Events;

namespace VehicleRental.Domain.Users
{
    public sealed class User : Entity<UserId>
    {
        private readonly List<Role> _roles = new();
        private User() { }
        private User(
            UserId id,
            FirstName firstName,
            LastName lastName,
            Email email,
            Password password) : base(id)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
        }

        public FirstName? FirstName { get; private set; }
        public LastName? LastName { get; private set; }
        public Email? Email { get; private set; }
        public Password? Password { get; private set; }
        public IReadOnlyCollection<Role>? Roles => _roles.ToList();

        public static User Create(FirstName firstName, LastName lastName, Email email, Password password)
        {
            var user = new User(UserId.New(), firstName, lastName, email, password);
            user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id!));
            user._roles.Add(Role.Client);
            return user;
        }

    }
}
