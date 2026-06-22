using Kanban.Communication.Email;
using Kanban.Communication.Email.Resource;
using Kanban.Domain.Services.MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Kanban.Infrastructure.Services.MailKit;

public class EmailService(IOptions<EmailSettings> settings) : IEmailService
{
    public async Task SendPasswordResetCode(string to, string userName, string code)
    {
        MimeMessage message = new();

        message.From.Add(address: MailboxAddress.Parse(text: settings.Value.From));
        message.To.Add(address: MailboxAddress.Parse(text: to));
        message.Subject = ResourceEmailMessage.TITLE_RESET_PASSWORD;

        message.Body = new TextPart(subtype: "html")
        {
            Text = ResetPasswordTemplate.Execute(username: userName, code: code)
        };

        using SmtpClient smtp = new();

        // Remover futuro
        smtp.Timeout = 60_000;
        smtp.CheckCertificateRevocation = false;

        await smtp.ConnectAsync(host: settings.Value.Host, port: settings.Value.Port, options: SecureSocketOptions.SslOnConnect);

        await smtp.AuthenticateAsync(userName: settings.Value.Username, password: settings.Value.Password);

        await smtp.SendAsync(message: message);
        await smtp.DisconnectAsync(quit: true);
    }

    public async Task SendWelcomeEmail(string to, string userName)
    {
        MimeMessage message = new();

        message.From.Add(address: MailboxAddress.Parse(text: settings.Value.From));
        message.To.Add(address: MailboxAddress.Parse(text: to));
        message.Subject = ResourceEmailMessage.TITLE_WELCOME;

        message.Body = new TextPart(subtype: "html")
        {
            Text = WelcomeTemplate.Execute(userName: userName)
        };

        using SmtpClient smtp = new();

        // Remover futuro
        smtp.Timeout = 60_000;
        smtp.CheckCertificateRevocation = false;

        await smtp.ConnectAsync(host: settings.Value.Host, port: settings.Value.Port, options: SecureSocketOptions.SslOnConnect);

        await smtp.AuthenticateAsync(userName: settings.Value.Username, password: settings.Value.Password);

        await smtp.SendAsync(message: message);
        await smtp.DisconnectAsync(quit: true);
    }
}