using jellytoring_api.Models.Email;
using System.Threading.Tasks;

namespace jellytoring_api.Service.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailRequest emailReq);
        Task SendEmailTemplateAsync(EmailTemplateRequest emailReq);
    }
}