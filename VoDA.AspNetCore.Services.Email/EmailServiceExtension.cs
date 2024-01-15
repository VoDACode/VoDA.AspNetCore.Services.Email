using Microsoft.Extensions.DependencyInjection;

namespace VoDA.AspNetCore.Services.Email
{
    public static class EmailServiceExtension
    {
        public static void AddEmailService(this IServiceCollection services, Action<EmailServiceOptions> options)
        {
            services.Configure(options);
            services.AddSingleton<IEmailService, EmailService>();
        }
    }
}
