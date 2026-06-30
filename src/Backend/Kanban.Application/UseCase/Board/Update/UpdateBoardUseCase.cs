using FluentValidation.Results;
using Kanban.Communication.Requests.Board;
using Kanban.Domain.Repositories;
using Kanban.Domain.Repositories.Boad;
using Kanban.Domain.Services.LoggedUser;
using Kanban.Exception;
using Kanban.Exception.ExceptionBase;

namespace Kanban.Application.UseCase.Board.Update;
using Domain.Entities;

public class UpdateBoardUseCase(
    IBoardWriteRepository writeRepository,
    ILoggedUser loggedUser,
    IUnitOfWork unitOfWork) : IUpdateBoardUseCase
{
    public async Task Execute(Guid id, RegisterBoardRequest request)
    {
        Board board = await GetValidatedBoard(id: id, request: request);
        board.Name = request.Name;
        
        writeRepository.Update(board: board);
        await unitOfWork.Commit();
    }
    
    private async Task<Board> GetValidatedBoard(Guid id, RegisterBoardRequest request)
    {
        BoardValidator validator = new();
        ValidationResult? result = await validator.ValidateAsync(instance: request);
        
        User user = await loggedUser.Get();
        
        Board? boardExists = await writeRepository.GetByTitle(title: request.Name, userId: user.Id, ignoreBoardId: id);
        if (boardExists != null)
        {
            result.Errors.Add(item: new ValidationFailure(propertyName: string.Empty, errorMessage: ResourceErrorMessage.BOARD_ALREADY_EXISTS));
        }
        
        if (!result.IsValid)
        {
            List<string> errors = result.Errors.Select(selector: error => error.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorsMessages: errors); 
        }
        
        Board? board = await writeRepository.GetById(id: id, userId: user.Id);
        if (board == null)
        {
            throw new NotFoundException(message: ResourceErrorMessage.BOARD_NOT_FOUND);
        }
        
        return board;
    }
}