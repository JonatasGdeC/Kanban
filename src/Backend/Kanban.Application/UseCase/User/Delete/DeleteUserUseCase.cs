using Kanban.Domain.Repositories;
using Kanban.Domain.Repositories.User;
using Kanban.Domain.Services.LoggedUser;

namespace Kanban.Application.UseCase.User.Delete;
using Domain.Entities;

public class DeleteUserUseCase(
    IUserWriteRepository writeRepository,
    ILoggedUser loggedUser,
    IUnitOfWork unitOfWork) : IDeleteUserUseCase
{
    public async Task Execute()
    {
        User user = await loggedUser.Get();
        writeRepository.Delete(user: user);
        await unitOfWork.Commit();
    }
}
