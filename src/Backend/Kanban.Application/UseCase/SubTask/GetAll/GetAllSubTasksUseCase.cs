using AutoMapper;
using Kanban.Communication.Dtos;
using Kanban.Communication.Responses.SubTask;
using Kanban.Domain.Repositories.SubTask;
using Kanban.Domain.Repositories.Task;
using Kanban.Domain.Services.LoggedUser;
using Kanban.Exception;
using Kanban.Exception.ExceptionBase;

namespace Kanban.Application.UseCase.SubTask.GetAll;
using Domain.Entities;

public class GetAllSubTasksUseCase(
    ISubTaskReadRepository readSubTaskRepository,
    ITaskReadRepository readTaskRepository,
    ILoggedUser loggedUser,
    IMapper mapper) : IGetAllSubTasksUseCase
{
    public async Task<GetAllSubTasksResponse> Execute(Guid taskId)
    {
        User user = await loggedUser.Get();
        await Validate(taskId: taskId, userId: user.Id);

        List<SubTask> subTasks = await readSubTaskRepository.GetAll(taskId: taskId, userId: user.Id);

        return new GetAllSubTasksResponse
        {
            ListSubTasks = mapper.Map<List<SubTaskDto>>(source: subTasks)
        };
    }

    private async Task Validate(Guid taskId, Guid userId)
    {
        TaskEntity? task = await readTaskRepository.GetById(id: taskId, userId: userId);
        if (task == null)
        {
            throw new NotFoundException(message: ResourceErrorMessage.TASK_NOT_FOUND);
        }
    }
}
