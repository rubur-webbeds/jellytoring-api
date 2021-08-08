namespace jellytoring_api.Models.Users
{
    public record CreateUser(
        string FullName,
        string Email,
        string Password,
        uint InterestId,
        string Institution,
        string CountryCode,
        bool GrantContactInfoPermission,
        bool GrantUibPermission);
}
