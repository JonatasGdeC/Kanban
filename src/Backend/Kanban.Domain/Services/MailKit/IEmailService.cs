namespace Kanban.Domain.Services.MailKit;

public interface IEmailService
{
    Task SendPasswordResetCode(string to, string userName, string code);
    Task SendWelcomeEmail(string to, string userName);
}