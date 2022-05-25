using jellytoring_api.Infrastructure.PasswordRecoveries;
using jellytoring_api.Models.PasswordRecovery;
using jellytoring_api.Models.Email;
using jellytoring_api.Models.Email.Template;
using jellytoring_api.Service.Email;
using jellytoring_api.Service.Users;
using System;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;

namespace jellytoring_api.Service.PasswordRecoveries
{
    public class PasswordRecoveryService
    {
        private readonly IUsersService _usersService;
        private readonly PasswordRecoveryRepository _passwordRecoveryRepository;
        private readonly IEmailService _emailService;
        private readonly double MaxHoursToConfirmEmail = 24;

        public PasswordRecoveryService(
            IUsersService usersService, 
            PasswordRecoveryRepository passwordRecoveryRepository,
            IEmailService emailService)
        {
            _usersService = usersService;
            _passwordRecoveryRepository = passwordRecoveryRepository;
            _emailService = emailService;
        }

        public async Task<bool> SendResetEmailTo(string email)
        {
            // check if user exists
            var user = await _usersService.GetAsync(email);
            if (user is null || !user.EmailConfirmed)
                return false;

            var guid = Guid.NewGuid();
            var passwordReset = new PasswordResetConfirmation { ConfirmationCode = guid, UserId = user.Id, IssuedAt = DateTime.Now };
            var confirmationId = await _passwordRecoveryRepository.CreateConfirmationAsync(passwordReset);

            if (confirmationId != 0)
            {
                var resetPasswordTemplate =
                new TemplateBuilder()
                .SetName("PasswordResetTemplate.html")
                .AddOption("CONFIRMATION_CODE")
                .WithValue(passwordReset.ConfirmationCode.ToString())
                .Build();

                var templateReq = new EmailTemplateRequest
                {
                    EmailRequest = new EmailRequest { To = email, Subject = "Jellytoring password reset" },
                    Template = resetPasswordTemplate
                };

                await _emailService.SendEmailTemplateAsync(templateReq);
                return true;
            }

            return false;
        }

        public async Task<bool> UpdatePasswordAsync(NewPasswordRequest passwordRequest)
        {
            if (await ConfirmationCodeIsValid(passwordRequest.ConfirmationCode))
            {
                var hashedPassword = BC.HashPassword(passwordRequest.Password);
                passwordRequest.Password = hashedPassword;

                return await _passwordRecoveryRepository.UpdatePasswordAsync(passwordRequest);
            }

            return false;
        }

        private async Task<bool> ConfirmationCodeIsValid(string confirmationCode)
        {
            var passwordResetConfirmation = await _passwordRecoveryRepository.GetConfirmationAsync(confirmationCode);
            if (passwordResetConfirmation is null)
            {
                return false;
            }

            // 24h to confirm the email
            return passwordResetConfirmation.IssuedAt.AddHours(MaxHoursToConfirmEmail) >= DateTime.Now;
        }
    }
}
