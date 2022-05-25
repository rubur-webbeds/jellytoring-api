using System;

namespace jellytoring_api.Models.PasswordRecovery
{
    public class PasswordResetConfirmation
    {
        public Guid ConfirmationCode { get; set; }
        public uint UserId { get; set; }
        public DateTime IssuedAt { get; set; }
    }
}
