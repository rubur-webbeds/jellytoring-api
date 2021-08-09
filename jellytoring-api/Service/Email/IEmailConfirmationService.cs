using System.Threading.Tasks;

namespace jellytoring_api.Service.Email
{
    public interface IEmailConfirmationService
    {
        Task SendEmailConfirmationAsync(string emailTo, uint userId);
    }
}