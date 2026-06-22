using AutoMapper;
using FluentValidation.Results;
using Kanban.Communication.Dtos;
using Kanban.Communication.Requests.SubTask;
using Kanban.Domain.Repositories;
using Kanban.Domain.Repositories.SubTask;
using Kanban.Domain.Repositories.Task;
using Kanban.Domain.Services.LoggedUser;
using Kanban.Exception;
using Kanban.Exception.ExceptionBase;

namespace Kanban.Application.UseCase.SubTask.Register;
using Domain.Entities;

public class RegisterSubTaskUseCase(
    ISubTaskWriteRepository writeSubTaskRepository,
    ITaskReadRepository readTaskRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILoggedUser loggedUser) : IRegisterSubTaskUseCase
{
    public async Task<SubTaskDto> Execute(Guid taskId, RegisterSubTaskRequest request)
    {
        User user = await loggedUser.Get();
        await Validate(request: request, taskId: taskId, userId: user.Id);

        SubTask subTask = mapper.Map<SubTask>(source: request);
        subTask.TaskId = taskId;

        await writeSubTaskRepository.Add(subTask: subTask);
        await unitOfWork.Commit();

        return mapper.Map<SubTaskDto>(source: subTask);
    }

    private async Task Validate(RegisterSubTaskRequest request, Guid taskId, Guid userId)
    {
        SubTaskValidator validator = new();
        ValidationResult? result = await validator.ValidateAsync(instance: request);

        if (!result.IsValid)
        {
            List<string> errors = result.Errors.Select(selector: error => error.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorsMessages: errors);
        }

        TaskEntity? task = await readTaskRepository.GetById(id: taskId, userId: userId);
        if (task == null)
        {
            throw new NotFoundException(message: ResourceErrorMessage.TASK_NOT_FOUND);
        }
    }
}
