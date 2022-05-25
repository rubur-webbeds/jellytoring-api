namespace jellytoring_api.Models.PasswordRecovery
{
    public class NewPasswordRequest
    {
        public string ConfirmationCode { get; set; }
        public string Password { get; set; }
    }
}
