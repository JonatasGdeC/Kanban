using AutoMapper;
using FluentValidation.Results;
using Kanban.Communication.Requests.Task;
using Kanban.Domain.Repositories;
using Kanban.Domain.Repositories.Column;
using Kanban.Domain.Repositories.Task;
using Kanban.Domain.Services.LoggedUser;
using Kanban.Exception;
using Kanban.Exception.ExceptionBase;

namespace Kanban.Application.UseCase.TaskEntity.Update;
using Domain.Entities;

public class UpdateTaskUseCase(
    ITaskWriteRepository writeRepository,
    ITaskReadRepository readRepository,
    IColumnReadRepository readColumnRepository,
    ILoggedUser loggedUser,
    IMapper mapper,
    IUnitOfWork unitOfWork) : IUpdateTaskUseCase
{
    public async Task Execute(Guid id, UpdateTaskRequest request)
    {
        TaskEntity task = await GetValidatedTask(id: id, request: request);

        mapper.Map(source: request, destination: task);

        writeRepository.Update(task: task);
        await unitOfWork.Commit();
    }

    private async Task<TaskEntity> GetValidatedTask(Guid id, UpdateTaskRequest request)
    {
        TaskValidator validator = new();
        ValidationResult? result = await validator.ValidateAsync(instance: request);

        if (!result.IsValid)
        {
            List<string> errors = result.Errors.Select(selector: error => error.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorsMessages: errors);
        }

        User user = await loggedUser.Get();
        TaskEntity? task = await writeRepository.GetById(id: id, userId: user.Id);
        if (task == null)
        {
            throw new NotFoundException(message: ResourceErrorMessage.TASK_NOT_FOUND);
        }
        
        Column? column = await readColumnRepository.GetById(id: request.ColumnId, userId: user.Id);
        if (column == null)
        {
            throw new NotFoundException(message: ResourceErrorMessage.COLUMN_NOT_FOUND);
        }

        bool existsTaskInPosition = await readRepository.ExistsTaskInPosition(columnId: request.ColumnId, position: request.Order, ignoreTaskId: task.Id);
        if (existsTaskInPosition)
        {
            throw new ErrorOnValidationException(errorsMessages: [ResourceErrorMessage.TASK_ALREADY_IN_POSITION]);
        }

        return task;
    }
}
