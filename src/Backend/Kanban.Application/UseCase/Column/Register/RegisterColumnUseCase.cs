using AutoMapper;
using FluentValidation.Results;
using Kanban.Communication.Dtos;
using Kanban.Communication.Requests.Column;
using Kanban.Domain.Repositories;
using Kanban.Domain.Repositories.Boad;
using Kanban.Domain.Repositories.Column;
using Kanban.Domain.Services.LoggedUser;
using Kanban.Exception;
using Kanban.Exception.ExceptionBase;

namespace Kanban.Application.UseCase.Column.Register;
using Domain.Entities;

public class RegisterColumnUseCase(
    IColumWriteRepository writeColumnRepository,
    IColumnReadRepository readColumnRepository,
    IBoardReadRepository readBoardRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILoggedUser loggedUser) : IRegisterColumnUseCase
{
    public async Task<ColumnDto> Execute(Guid boardId, RegisterColumnRequest request)
    {
        User user = await loggedUser.Get();
        await Validate(request: request, boardId: boardId, userId: user.Id);

        List<Column> columns = await readColumnRepository.GetAll(boardId: boardId, userId: user.Id);
        Column column = mapper.Map<Column>(source: request);
        column.BoardId = boardId;
        column.Order = columns.Count + 1;

        await writeColumnRepository.Add(column: column);
        await unitOfWork.Commit();

        return mapper.Map<ColumnDto>(source: column);
    }

    private async Task Validate(RegisterColumnRequest request, Guid boardId, Guid userId)
    {
        ColumnValidator validator = new();
        ValidationResult? result = await validator.ValidateAsync(instance: request);

        if (!result.IsValid)
        {
            List<string> errors = result.Errors.Select(selector: error => error.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorsMessages: errors);
        }
        
        Board? board = await readBoardRepository.GetById(id: boardId, userId: userId);
        if (board == null)
        {
            throw new NotFoundException(message: ResourceErrorMessage.BOARD_NOT_FOUND);
        }
    }
}
