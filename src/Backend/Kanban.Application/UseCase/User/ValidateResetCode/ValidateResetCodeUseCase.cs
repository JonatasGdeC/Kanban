using Kanban.Communication.Requests.User;
using Kanban.Communication.Responses.User;
using Kanban.Domain.Repositories;
using Kanban.Domain.Repositories.PasswordResetCode;
using Kanban.Domain.Repositories.User;
using Kanban.Domain.Security.Cryptography;
using Kanban.Domain.Security.Tokens;
using Kanban.Exception;
using Kanban.Exception.ExceptionBase;

namespace Kanban.Application.UseCase.User.ValidateResetCode;

using Domain.Entities;

public class ValidateResetCodeUseCase(
    IUserReadRepository userReadRepository,
    IPasswordResetCodeRepository passwordResetCodeRepository,
    IEncrypter encrypter,
    IPasswordResetTokenGenerator passwordResetTokenGenerator,
    IUnitOfWork unitOfWork) : IValidateResetCodeUseCase
{
    public async Task<ValidateResetCodeResponse> Execute(ValidateResetCodeRequest request)
    {
        User user = await userReadRepository.GetByEmail(email: request.Email) ??
                    throw new BadRequestException(message: ResourceErrorMessage.CODE_INVALID);
        
        PasswordResetCode passwordResetCode = await passwordResetCodeRepository.GetByUserId(userId: user.Id) ??
                                              throw new BadRequestException(message: ResourceErrorMessage.CODE_INVALID);

        if (!passwordResetCode.IsValid)
        {
            throw new BadRequestException(message: ResourceErrorMessage.CODE_INVALID);
        }

        bool isValidCode = encrypter.Verify(value: request.Code, hash: passwordResetCode.CodeHash);
        if (!isValidCode)
        {
            passwordResetCode.Attempts++;
            passwordResetCodeRepository.Update(passwordResetCode: passwordResetCode);
            await unitOfWork.Commit();
            throw new BadRequestException(message: ResourceErrorMessage.CODE_INVALID);
        }

        return new ValidateResetCodeResponse
        {
            TokenResetPassword = passwordResetTokenGenerator.Generate(user: user)
        };
    }
}