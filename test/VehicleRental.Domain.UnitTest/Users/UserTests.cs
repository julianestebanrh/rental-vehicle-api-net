using FluentAssertions;
using VehicleRental.Domain.Roles;
using VehicleRental.Domain.UnitTest.Infrastructure;
using VehicleRental.Domain.Users;
using VehicleRental.Domain.Users.Events;
using Xunit;

namespace VehicleRental.Domain.UnitTest.Users
{
    public class UserTests : BaseTest
    {
        [Fact]
        public void Create_Should_SetPropertyValues()
        {
            // Act
            var user = User.Create(UserMock.FirstName, UserMock.LastName, UserMock.Email, UserMock.Password);

            // Assert
            user.FirstName.Should().Be(UserMock.FirstName);
            user.LastName.Should().Be(UserMock.LastName);
            user.Email.Should().Be(UserMock.Email);
            user.Password.Should().Be(UserMock.Password);
        }

        [Fact]
        public void Create_Should_RaiseUserCreatedDomainEvent()
        {
            // Act
            var user = User.Create(UserMock.FirstName, UserMock.LastName, UserMock.Email, UserMock.Password);

            // Assert
            var userCreatedDomainEvent = AssertDomainEventWasPublished<UserCreatedDomainEvent>(user);

            userCreatedDomainEvent.UserId.Should().Be(user.Id);
        }

        [Fact]
        public void Create_Should_AddRegisteredRoleToUser()
        {
            // Act
            var user = User.Create(UserMock.FirstName, UserMock.LastName, UserMock.Email, UserMock.Password);

            // Assert
            user.Roles.Should().Contain(Role.Client);
        }
    }
}
