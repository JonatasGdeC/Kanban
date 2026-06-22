using AutoMapper;
using FluentValidation.Results;
using Kanban.Communication.Requests.Column;
using Kanban.Domain.Repositories;
using Kanban.Domain.Repositories.Column;
using Kanban.Domain.Services.LoggedUser;
using Kanban.Exception;
using Kanban.Exception.ExceptionBase;

namespace Kanban.Application.UseCase.Column.Update;
using Domain.Entities;

public class UpdateColumnUseCase(
    IColumWriteRepository writeRepository,
    ILoggedUser loggedUser,
    IMapper mapper,
    IUnitOfWork unitOfWork) : IUpdateColumnUseCase
{
    public async Task Execute(Guid id, UpdateColumnRequest request)
    {
        Column column = await GetValidatedColumn(id: id, request: request);

        mapper.Map(source: request, destination: column);
        
        await writeRepository.Update(column: column);
        await unitOfWork.Commit();
    }

    private async Task<Column> GetValidatedColumn(Guid id, UpdateColumnRequest request)
    {
        ColumnValidator validator = new();
        ValidationResult? result = await validator.ValidateAsync(instance: request);
        
        if (!result.IsValid)
        {
            List<string> errors = result.Errors.Select(selector: error => error.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorsMessages: errors);
        }

        User user = await loggedUser.Get();
        Column? column = await writeRepository.GetById(id: id, userId: user.Id);
        if (column == null)
        {
            throw new NotFoundException(message: ResourceErrorMessage.COLUMN_NOT_FOUND);
        }
        
        return column;
    }
}
