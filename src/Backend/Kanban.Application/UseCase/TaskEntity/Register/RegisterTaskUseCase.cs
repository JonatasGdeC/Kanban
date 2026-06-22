using AutoMapper;
using FluentValidation.Results;
using Kanban.Communication.Dtos;
using Kanban.Communication.Requests.Task;
using Kanban.Domain.Repositories;
using Kanban.Domain.Repositories.Column;
using Kanban.Domain.Repositories.Task;
using Kanban.Domain.Services.LoggedUser;
using Kanban.Exception;
using Kanban.Exception.ExceptionBase;

namespace Kanban.Application.UseCase.TaskEntity.Register;
using Domain.Entities;

public class RegisterTaskUseCase(
    ITaskWriteRepository writeTaskRepository,
    ITaskReadRepository readTaskRepository,
    IColumnReadRepository readColumnRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILoggedUser loggedUser) : IRegisterTaskUseCase
{
    public async Task<TaskDto> Execute(Guid columnId, RegisterTaskRequest request)
    {
        User user = await loggedUser.Get();
        await Validate(request: request, columnId: columnId, userId: user.Id);

        List<TaskEntity> tasks = await readTaskRepository.GetAll(columnId: columnId, userId: user.Id);
        TaskEntity task = mapper.Map<TaskEntity>(source: request);
        task.ColumnId = columnId;
        task.Order = tasks.Count + 1;

        await writeTaskRepository.Add(task: task);
        await unitOfWork.Commit();

        return mapper.Map<TaskDto>(source: task);
    }

    private async Task Validate(RegisterTaskRequest request, Guid columnId, Guid userId)
    {
        TaskValidator validator = new();
        ValidationResult? result = await validator.ValidateAsync(instance: request);

        if (!result.IsValid)
        {
            List<string> errors = result.Errors.Select(selector: error => error.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorsMessages: errors);
        }

        Column? column = await readColumnRepository.GetById(id: columnId, userId: userId);
        if (column == null)
        {
            throw new NotFoundException(message: ResourceErrorMessage.COLUMN_NOT_FOUND);
        }
    }
}
