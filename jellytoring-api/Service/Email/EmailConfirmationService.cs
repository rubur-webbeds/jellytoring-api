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
                // TODO: generate email body with guid and url
                var body = $"whats uuuuuup. {guid}";
                var email = new EmailRequest { To = emailTo, Subject = "Jellytoring email confirmation", Body = body };
                await _emailService.SendEmailAsync(email);
            }
        }

        public Task<bool> ConfirmEmailAsync(string confirmationCode) => _emailConfirmationRepository.ConfirmEmailAsync(confirmationCode);
    }
}
