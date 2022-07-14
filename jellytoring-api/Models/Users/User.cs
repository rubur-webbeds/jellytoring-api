namespace jellytoring_api.Models.Users
{
    public record User(
        uint Id,
        string FullName,
        string Email,
        uint InterestId,
        string Institution,
        string CountryCode,
        bool GrantContactInfoPermission,
        bool GrantUibPermission,
        bool EmailConfirmed,
        bool Active);

    public record UserDetails
    {
        public uint Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Institution { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool Active { get; set; }
        public string InterestName { get; set; }
        public string CountryName { get; set; }
        public string RoleCode { get; set; }
        public int InferencesCount { get; set; }
    }
}
