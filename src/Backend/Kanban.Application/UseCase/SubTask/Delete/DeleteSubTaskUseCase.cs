using Kanban.Domain.Repositories;
using Kanban.Domain.Repositories.SubTask;
using Kanban.Domain.Services.LoggedUser;
using Kanban.Exception;
using Kanban.Exception.ExceptionBase;

namespace Kanban.Application.UseCase.SubTask.Delete;
using Domain.Entities;

public class DeleteSubTaskUseCase(
    ISubTaskWriteRepository writeRepository,
    ILoggedUser loggedUser,
    IUnitOfWork unitOfWork) : IDeleteSubTaskUseCase
{
    public async Task Execute(Guid id)
    {
        SubTask subTask = await GetValidatedSubTask(id: id);
        writeRepository.Delete(subTask: subTask);
        await unitOfWork.Commit();
    }

    private async Task<SubTask> GetValidatedSubTask(Guid id)
    {
        User user = await loggedUser.Get();
        SubTask? subTask = await writeRepository.GetById(id: id, userId: user.Id);

        if (subTask == null)
        {
            throw new NotFoundException(message: ResourceErrorMessage.SUBTASK_NOT_FOUND);
        }

        return subTask;
    }
}
