namespace VoDA.AspNetCore.Services.Email
{
    public interface IEmailService
    {
        public Task SendEmail(string email, string subject, string message, bool isHtml = false);
        public Task SendEmailUseTemplate(string email, string tepmlateName, Dictionary<string, string>? parameters = null, string? subject = null);
    }
}
