using Kanban.Domain.Repositories;
using Kanban.Domain.Repositories.Task;
using Kanban.Domain.Services.LoggedUser;
using Kanban.Exception;
using Kanban.Exception.ExceptionBase;

namespace Kanban.Application.UseCase.TaskEntity.Delete;
using Domain.Entities;

public class DeleteTaskUseCase(
    ITaskWriteRepository writeRepository,
    ILoggedUser loggedUser,
    IUnitOfWork unitOfWork) : IDeleteTaskUseCase
{
    public async Task Execute(Guid id)
    {
        TaskEntity task = await GetValidatedTask(id: id);
        writeRepository.Delete(task: task);
        await unitOfWork.Commit();
    }

    private async Task<TaskEntity> GetValidatedTask(Guid id)
    {
        User user = await loggedUser.Get();
        TaskEntity? task = await writeRepository.GetById(id: id, userId: user.Id);

        if (task == null)
        {
            throw new NotFoundException(message: ResourceErrorMessage.TASK_NOT_FOUND);
        }

        return task;
    }
}
