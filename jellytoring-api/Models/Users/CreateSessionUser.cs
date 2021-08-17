namespace jellytoring_api.Models.Users
{
    public record CreateSessionUser(
        uint Id,
        string Email,
        string Password,
        bool EmailConfirmed);
}
