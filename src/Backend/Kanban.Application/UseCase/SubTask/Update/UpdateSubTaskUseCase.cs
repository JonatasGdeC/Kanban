using AutoMapper;
using FluentValidation.Results;
using Kanban.Communication.Requests.SubTask;
using Kanban.Domain.Repositories;
using Kanban.Domain.Repositories.SubTask;
using Kanban.Domain.Services.LoggedUser;
using Kanban.Exception;
using Kanban.Exception.ExceptionBase;

namespace Kanban.Application.UseCase.SubTask.Update;
using Domain.Entities;

public class UpdateSubTaskUseCase(
    ISubTaskWriteRepository writeRepository,
    ILoggedUser loggedUser,
    IMapper mapper,
    IUnitOfWork unitOfWork) : IUpdateSubTaskUseCase
{
    public async Task Execute(Guid id, UpdateSubTaskRequest request)
    {
        SubTask subTask = await GetValidatedSubTask(id: id, request: request);

        mapper.Map(source: request, destination: subTask);

        writeRepository.Update(subTask: subTask);
        await unitOfWork.Commit();
    }

    private async Task<SubTask> GetValidatedSubTask(Guid id, UpdateSubTaskRequest request)
    {
        SubTaskValidator validator = new();
        ValidationResult? result = await validator.ValidateAsync(instance: request);

        if (!result.IsValid)
        {
            List<string> errors = result.Errors.Select(selector: error => error.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorsMessages: errors);
        }

        User user = await loggedUser.Get();
        SubTask? subTask = await writeRepository.GetById(id: id, userId: user.Id);
        if (subTask == null)
        {
            throw new NotFoundException(message: ResourceErrorMessage.SUBTASK_NOT_FOUND);
        }

        return subTask;
    }
}
