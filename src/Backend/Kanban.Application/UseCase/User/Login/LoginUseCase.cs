using AutoMapper;
using Kanban.Communication.Dtos;
using Kanban.Communication.Requests.User;
using Kanban.Communication.Responses.User;
using Kanban.Domain.Repositories.User;
using Kanban.Domain.Security.Cryptography;
using Kanban.Domain.Security.Tokens;
using Kanban.Exception.ExceptionBase;

namespace Kanban.Application.UseCase.User.Login;
using Domain.Entities;

public class LoginUseCase(
    IUserReadRepository readRepository,
    IEncrypter passwordEncrypter,
    IAccessTokenGenerator tokenGenerator,
    IMapper mapper) : ILoginUseCase
{
    public async Task<LoginResponse> Execute(LoginRequest request)
    {
        User? user = await readRepository.GetByEmail(email: request.Email);

        if (user == null || !passwordEncrypter.Verify(value: request.Password, hash: user.Password))
        {
            throw new InvalidLoginException();
        }

        return new LoginResponse
        {
            User = mapper.Map<UserDto>(source: user),
            Token = tokenGenerator.Generate(user: user)
        };
    }
}
