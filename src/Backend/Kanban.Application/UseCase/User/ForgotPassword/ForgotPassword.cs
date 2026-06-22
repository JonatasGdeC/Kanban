using Kanban.Communication.Requests.User;
using Kanban.Domain.Repositories;
using Kanban.Domain.Repositories.PasswordResetCode;
using Kanban.Domain.Repositories.User;
using Kanban.Domain.Security.CodeGenerator;
using Kanban.Domain.Security.Cryptography;
using Kanban.Domain.Services.MailKit;

namespace Kanban.Application.UseCase.User.ForgotPassword;
using Domain.Entities;

public class ForgotPassword(
    IUserReadRepository userReadRepository,
    IPasswordResetCodeRepository passwordResetCodeRepository,
    IEmailService emailService,
    IEncrypter encrypter,
    ICodeGenerator codeGenerator,
    IUnitOfWork unitOfWork) : IForgotPassword
{
    public async Task Execute(ForgotPasswordRequest request)
    {
        User? user = await userReadRepository.GetByEmail(email: request.Email);
        if (user != null)
        {
            string code = codeGenerator.Generate();
            
            PasswordResetCode? resetCodeByUser = await passwordResetCodeRepository.GetByUserId(userId: user.Id);
            if (resetCodeByUser != null)
            {
                passwordResetCodeRepository.Remove(passwordResetCode: resetCodeByUser);
            }
            
            PasswordResetCode passwordResetCode = new()
            {
                UserId = user.Id,
                CodeHash = encrypter.Encrypt(value: code),
                Attempts = 0,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(value: 10),
            };

            await passwordResetCodeRepository.Add(passwordResetCode: passwordResetCode);
            await unitOfWork.Commit();
            
            await emailService.SendPasswordResetCode(to: user.Email, userName: user.Name, code: code);
        }
    }
}