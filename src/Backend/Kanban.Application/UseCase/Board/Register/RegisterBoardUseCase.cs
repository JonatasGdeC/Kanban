using AutoMapper;
using FluentValidation.Results;
using Kanban.Communication.Dtos;
using Kanban.Communication.Requests.Board;
using Kanban.Domain.Repositories;
using Kanban.Domain.Repositories.Boad;
using Kanban.Domain.Services.LoggedUser;
using Kanban.Exception;
using Kanban.Exception.ExceptionBase;

namespace Kanban.Application.UseCase.Board.Register;
using Domain.Entities;

public class RegisterBoardUseCase(
    IBoardReadRepository readRepository, 
    IBoardWriteRepository whiteRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILoggedUser loggedUser) : IRegisterBoardUseCase
{
    public async Task<BoardDto> Execute(RegisterBoardRequest request)
    {
        User user = await loggedUser.Get();
        await Validate(request: request, userId: user.Id);
        
        Board board = mapper.Map<Board>(source: request);
        board.UserId = user.Id;
        
        await whiteRepository.Add(board: board);
        await unitOfWork.Commit();
        
        return mapper.Map<BoardDto>(source: board);
    }

    private async Task Validate(RegisterBoardRequest request, Guid userId)
    {
        BoardValidator boardValidator = new();
        ValidationResult? result = await boardValidator.ValidateAsync(instance: request);

        Board? boardExists = await readRepository.GetByTitle(title: request.Name, userId: userId);
        if (boardExists != null)
        {
            result.Errors.Add(item: new ValidationFailure(propertyName: string.Empty, errorMessage: ResourceErrorMessage.BOARD_ALREADY_EXISTS));
        }
        
        if (!result.IsValid)
        {
            List<string> errors = result.Errors.Select(selector: error => error.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorsMessages: errors); 
        }
    }
}