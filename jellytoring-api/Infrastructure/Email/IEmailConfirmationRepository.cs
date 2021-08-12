using jellytoring_api.Models.Email;
using System.Threading.Tasks;

namespace jellytoring_api.Infrastructure.Email
{
    public interface IEmailConfirmationRepository
    {
        Task<uint> CreateConfirmationAsync(EmailConfirmation emailConfirmation);
        Task<bool> ConfirmEmailAsync(string confirmationCode);
        Task<EmailConfirmation> GetConfirmationAsync(string confirmationCode);
    }
}