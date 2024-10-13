namespace VehicleRental.Application.Users.GetAllUser
{
    public sealed record GetAllUserQueryDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public HashSet<string> Permissions { get; set; }
    }


}
