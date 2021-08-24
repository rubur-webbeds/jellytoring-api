namespace jellytoring_api.Models.Email
{
    public class EmailTemplateRequest
    {
        public EmailRequest EmailRequest { get; set; }
        public Template.Template Template { get; set; }
    }
}
