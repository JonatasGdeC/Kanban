using AutoMapper;
using Kanban.Communication.Dtos;
using Kanban.Domain.Services.LoggedUser;

namespace Kanban.Application.UseCase.User.Get;
using Domain.Entities;

public class GetUserUseCase(ILoggedUser loggedUser, IMapper mapper) : IGetUserUseCase
{
    public async Task<UserDto> Execute()
    {
        User response = await loggedUser.Get();
        return mapper.Map<UserDto>(source: response);
    }
}