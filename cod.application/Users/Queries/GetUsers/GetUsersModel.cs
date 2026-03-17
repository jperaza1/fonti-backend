namespace cod.application.Users.Queries.GetUsers
{
    public class GetUsersModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserTypeText { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public string Carrier { get; set; }
        public string Cafeteria { get; set; }
    }
}