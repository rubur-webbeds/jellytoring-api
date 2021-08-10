using System;

namespace jellytoring_api.Models.Email
{
    public class EmailConfirmation
    {
        public Guid ConfirmationCode { get; set; }
        public uint UserId { get; set; }
        public DateTime IssuedAt { get; set; }
    }
}
