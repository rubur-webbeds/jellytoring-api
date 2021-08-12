namespace jellytoring_api.Models.Users
{
    public record CreateSessionUser(
        string Email,
        string Password,
        bool EmailConfirmed);
}
