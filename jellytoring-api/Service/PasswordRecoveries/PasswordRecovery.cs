namespace jellytoring_api.Service.PasswordRecoveries
{
    public class PasswordRecovery
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmationCode { get; set; }
    }
}
