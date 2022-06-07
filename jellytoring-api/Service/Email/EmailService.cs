using jellytoring_api.Models.Email;
using jellytoring_api.Models.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using MimeKit;
using System.IO;
using System.Threading.Tasks;

namespace jellytoring_api.Service.Email
{
    public class EmailService : IEmailService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly EmailSettings _mailSettings;
        public EmailService(IOptions<EmailSettings> mailSettings, IWebHostEnvironment webHostEnvironment)
        {
            _mailSettings = mailSettings.Value;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task SendEmailAsync(EmailRequest emailReq)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Email);
            email.To.Add(MailboxAddress.Parse(emailReq.To));
            email.Subject = emailReq.Subject;
            var builder = new BodyBuilder();
            builder.HtmlBody = emailReq.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Email, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

        public async Task SendEmailTemplateAsync(EmailTemplateRequest emailReq)
        {
            string FilePath = Path.Combine(_webHostEnvironment.WebRootPath, "Templates", emailReq.Template.Name);
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();

            // replace all the template options
            foreach(var key in emailReq.Template.Options.Options.Keys)
            {
                MailText = MailText.Replace(key, emailReq.Template.Options.Options[key]);
            }

            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Email);
            email.To.Add(MailboxAddress.Parse(emailReq.EmailRequest.To));
            email.Subject = emailReq.EmailRequest.Subject;
            var builder = new BodyBuilder();
            builder.HtmlBody = MailText;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            //smtp.Authenticate(_mailSettings.Email, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }


    }
}
