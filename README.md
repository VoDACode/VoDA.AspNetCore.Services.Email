[![Stand With Ukraine](https://raw.githubusercontent.com/VoDACode/VoDA.AspNetCore.Services.Email/master/docs/img/banner2-direct.svg)](https://vshymanskyy.github.io/StandWithUkraine/)

[![nuget](https://img.shields.io/static/v1?label=NuGet&message=VoDA.AspNetCore.Services.Email&color=blue&logo=nuget)](https://www.nuget.org/packages/VoDA.AspNetCore.Services.Email)

# Description

This library provides a simple way to send emails from your ASP.NET Core application.

# Installation

Install the [NuGet package](https://www.nuget.org/packages/VoDA.AspNetCore.Services.Email) into your project.

# Usage

## 1. Configure SMTP

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddEmailService(options =>
    {
        options.Host = "smtp.example.com";
        options.Port = 587;
        options.Email = "email@example.com";
        options.Password = "password";
        options.DisplayName = "Example";
        options.EnableSsl = true;
        options.UseDefaultCredentials = false;
        options.EmailTemplatesFolder = "EmailTemplates";
    });
}
```

## 2. Create email template

Create a file `EmailTemplates/MyTemplate.html` with the following content:

```html
<!DOCUMENT html>
<html>
<head>
    <title>Activate Account</title>
    <meta name="subject" content="Activate Account"/>
</head>
<body>
    <h1>Activate Account</h1>
    <p>Hi {{firstName}},</p>
    <p>Thank you for registering with us. Please click on the link below to activate your account.</p>
    <p><a href="{{link}}">Activate Account</a></p>
    <p>Regards,</p>
    <p>Team</p>
</body>
</html>
```

## 3. Send email

```csharp
public class MyController : Controller
{
    private readonly IEmailService _emailService;

    public MyController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task<IActionResult> SendEmail()
    {
        Dictionary<string, string> model = new Dictionary<string, string>
        {
            { "firstName", "John" },
            { "link", "https://example.com/activate-account" }
        };

        await _emailService.SendEmailUseTemplate("some.email@example.com", "MyTemplate", model);

        return Ok();
    }
}
```

# License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
