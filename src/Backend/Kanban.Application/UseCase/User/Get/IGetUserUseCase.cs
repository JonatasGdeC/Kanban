using Kanban.Communication.Dtos;

namespace Kanban.Application.UseCase.User.Get;

public interface IGetUserUseCase
{
    Task<UserDto> Execute();
}