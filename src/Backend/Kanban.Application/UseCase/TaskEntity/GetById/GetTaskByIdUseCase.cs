using AutoMapper;
using Kanban.Communication.Dtos;
using Kanban.Communication.Responses.Task;
using Kanban.Domain.Repositories.Task;
using Kanban.Domain.Services.LoggedUser;
using Kanban.Exception;
using Kanban.Exception.ExceptionBase;

namespace Kanban.Application.UseCase.TaskEntity.GetById;
using Domain.Entities;

public class GetTaskByIdUseCase(
    ITaskReadRepository readRepository,
    ILoggedUser loggedUser,
    IMapper mapper) : IGetTaskByIdUseCase
{
    public async Task<GetTaskByIdResponse> Execute(Guid id)
    {
        TaskEntity task = await GetValidatedTask(id: id);

        return new GetTaskByIdResponse
        {
            Task = mapper.Map<TaskDto>(source: task),
            SubTasks = mapper.Map<List<SubTaskDto>>(source: task.SubTasks)
        };
    }

    private async Task<TaskEntity> GetValidatedTask(Guid id)
    {
        User user = await loggedUser.Get();
        TaskEntity? task = await readRepository.GetById(id: id, userId: user.Id);

        if (task == null)
        {
            throw new NotFoundException(message: ResourceErrorMessage.TASK_NOT_FOUND);
        }

        return task;
    }
}
