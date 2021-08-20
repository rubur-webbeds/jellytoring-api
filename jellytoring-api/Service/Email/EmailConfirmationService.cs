using jellytoring_api.Infrastructure.Email;
using jellytoring_api.Models.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jellytoring_api.Service.Email
{
    public class EmailConfirmationService : IEmailConfirmationService
    {
        private readonly IEmailService _emailService;
        private readonly IEmailConfirmationRepository _emailConfirmationRepository;
        private readonly double MaxHoursToConfirmEmail = 24;

        public EmailConfirmationService(IEmailService emailService, IEmailConfirmationRepository emailConfirmationRepository)
        {
            _emailService = emailService;
            _emailConfirmationRepository = emailConfirmationRepository;
        }

        public async Task SendEmailConfirmationAsync(string emailTo, uint userId)
        {
            var guid = Guid.NewGuid();
            var emailConfirmation = new EmailConfirmation { ConfirmationCode = guid, UserId = userId, IssuedAt = DateTime.Now };
            var confirmationId = await _emailConfirmationRepository.CreateConfirmationAsync(emailConfirmation);

            if (confirmationId != 0)
            {
                var body = emailConfirmation.ConfirmationCode.ToString();
                var email = new EmailRequest { To = emailTo, Subject = "Jellytoring email verification", Body = body };
                await _emailService.SendEmailTemplateAsync(email);
            }
        }

        public async Task<bool> ConfirmEmailAsync(string confirmationCode)
        {
            if (await ConfirmationCodeIsValid(confirmationCode))
            {
                return await _emailConfirmationRepository.ConfirmEmailAsync(confirmationCode);
            }

            return false;
        }

        private async Task<bool> ConfirmationCodeIsValid(string confirmationCode)
        {
            var emailConfirmation = await _emailConfirmationRepository.GetConfirmationAsync(confirmationCode);
            if(emailConfirmation is null)
            {
                return false;
            }

            // 24h to confirm the email
            return emailConfirmation.IssuedAt.AddHours(MaxHoursToConfirmEmail) >= DateTime.Now;
        }
    }
}
