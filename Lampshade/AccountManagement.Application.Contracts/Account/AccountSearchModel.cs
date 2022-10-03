namespace AccountManagement.Application.Contracts.Account
{
    public class AccountSearchModel
    {
        public long Id { get; set; }
        public string Fullname { get; set; }
        public string Username { get; set; }
        public string Mobile { get; set; }
        public long RoleId { get; set; }
    }
}