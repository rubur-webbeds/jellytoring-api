namespace jellytoring_api.Models.Users
{
    public class CreateUser
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public uint InterestId { get; set; }
        public string Institution { get; set; }
        public string CountryCode { get; set; }
        public bool GrantContactInfoPermission { get; set; }
        public bool GrantUibPermission { get; set; }
    }
}
