using AutoMapper;
using Kanban.Communication.Dtos;
using Kanban.Communication.Responses.Task;
using Kanban.Domain.Repositories.Column;
using Kanban.Domain.Repositories.Task;
using Kanban.Domain.Services.LoggedUser;
using Kanban.Exception;
using Kanban.Exception.ExceptionBase;

namespace Kanban.Application.UseCase.TaskEntity.GetAll;
using Domain.Entities;

public class GetAllTasksUseCase(
    ITaskReadRepository readTaskRepository,
    IColumnReadRepository readColumnRepository,
    ILoggedUser loggedUser,
    IMapper mapper) : IGetAllTasksUseCase
{
    public async Task<GetAllTasksResponse> Execute(Guid columnId)
    {
        User user = await loggedUser.Get();
        await Validate(columnId: columnId, userId: user.Id);

        List<TaskEntity> tasks = await readTaskRepository.GetAll(columnId: columnId, userId: user.Id);

        return new GetAllTasksResponse
        {
            ListTasks = mapper.Map<List<TaskDto>>(source: tasks)
        };
    }

    private async Task Validate(Guid columnId, Guid userId)
    {
        Column? column = await readColumnRepository.GetById(id: columnId, userId: userId);
        if (column == null)
        {
            throw new NotFoundException(message: ResourceErrorMessage.COLUMN_NOT_FOUND);
        }
    }
}
