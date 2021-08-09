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
}
