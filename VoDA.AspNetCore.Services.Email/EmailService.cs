using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using System.Text;

namespace VoDA.AspNetCore.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailServiceOptions configuration;

        private string EmailTemplatesFouder => Path.IsPathRooted(configuration.EmailTemplatesFolder) ?
            configuration.EmailTemplatesFolder :
            Path.Combine(Directory.GetCurrentDirectory(), configuration.EmailTemplatesFolder);

        public EmailService(IOptions<EmailServiceOptions> configuration)
        {
            this.configuration = configuration.Value;
            if (!Directory.Exists(EmailTemplatesFouder))
            {
                Directory.CreateDirectory(EmailTemplatesFouder);
            }
        }

        public async Task SendEmail(string email, string subject, string message, bool isHtml = false)
        {
            using (SmtpClient client = Connect())
            {
                using (MailMessage mailMessage = new MailMessage(configuration.Email, email, subject, message))
                {
                    mailMessage.Sender = new MailAddress(configuration.Email, configuration.DisplayName, Encoding.UTF8);
                    mailMessage.IsBodyHtml = isHtml;
                    await client.SendMailAsync(mailMessage);
                }
            }
        }

        public async Task SendEmailUseTemplate(string email, string tepmlateName, Dictionary<string, string>? parameters = null, string? subject = null)
        {
            string templatePath = Path.Combine(EmailTemplatesFouder, tepmlateName);
            if (!File.Exists(templatePath))
            {
                throw new FileNotFoundException($"Template {tepmlateName} not found");
            }

            string template = File.ReadAllText(templatePath);
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    template = template.Replace($@"{{{{{parameter.Key}}}}}", parameter.Value);
                }
            }

            if (subject == null)
            {
                subject = Regex.Match(template, @"<meta name=""subject"" content=""(.*)""").Groups[1].Value;
            }

            await SendEmail(email, subject, template, true);
        }

        private SmtpClient Connect()
        {
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = configuration.Host;
            smtpClient.Port = configuration.Port;
            smtpClient.EnableSsl = configuration.EnableSsl;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = configuration.UseDefaultCredentials;
            smtpClient.Credentials = new NetworkCredential(configuration.Email, configuration.Password);
            return smtpClient;
        }
    }
}
