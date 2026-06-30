using CommomTestsUtilies.Entities;
using CommomTestsUtilies.LoggedUser;
using CommomTestsUtilies.Repositories;
using CommomTestsUtilies.Repositories.Board;
using CommomTestsUtilies.Requests;
using FluentAssertions;
using FluentAssertions.Specialized;
using Kanban.Application.UseCase.Board.Update;
using Kanban.Communication.Requests.Board;
using Kanban.Domain.Repositories;
using Kanban.Domain.Services.LoggedUser;
using Kanban.Exception;
using Kanban.Exception.ExceptionBase;

namespace UseCases.Tests.Board;
using Kanban.Domain.Entities;
using System.Threading.Tasks;

public class UpdateBoardUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        User user = UserBuilder.Build();
        Board board = BoardBuilder.Build(userId: user.Id);
        RegisterBoardRequest request = RegisterBoardRequestBuilder.Build();
        UpdateBoardUseCase useCase = CreateUseCase(user: user, currentBoard: board);
        
        Func<Task> act = async () => await  useCase.Execute(id: board.Id, request: request);
        
        await act.Should().NotThrowAsync();
    }
    
    [Fact]
    public async Task Error_Board_Not_Found()
    {
        User user = UserBuilder.Build();
        Board board = BoardBuilder.Build();
        RegisterBoardRequest request = RegisterBoardRequestBuilder.Build();
        UpdateBoardUseCase useCase = CreateUseCase(user: user, currentBoard: board);
        
        Func<Task> act = async () => await  useCase.Execute(id: board.Id, request: request);
        
        ExceptionAssertions<NotFoundException>? result = await act.Should().ThrowAsync<NotFoundException>();
        result.Where(exceptionExpression: ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessage.BOARD_NOT_FOUND));
    }
    
    [Fact]
    public async Task Error_Board_Already_Exists()
    {
        User user = UserBuilder.Build();
        RegisterBoardRequest request = RegisterBoardRequestBuilder.Build();
        Board board = BoardBuilder.Build(userId: user.Id, boardName: request.Name);
        UpdateBoardUseCase useCase = CreateUseCase(user: user, currentBoard: board, boardExists: board);
        
        Func<Task> act = async () => await  useCase.Execute(id: board.Id, request: request);
     
        ExceptionAssertions<ErrorOnValidationException>? result = await act.Should().ThrowAsync<ErrorOnValidationException>();
        result.Where(exceptionExpression: ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessage.BOARD_ALREADY_EXISTS));
    }
    
    private UpdateBoardUseCase CreateUseCase(User user, Board currentBoard, Board? boardExists = null)
    {
        BoardWriteRepositoryBuilder whiteRepository = new();
        ILoggedUser loggedUser = LoggedUserBuilder.Build(user: user);
        IUnitOfWork unitOfWork = UnitOfWorkBuilder.Build();

        if (boardExists != null)
        {
            whiteRepository.GetByTitle(board: boardExists);
        }
        
        return new UpdateBoardUseCase(
            writeRepository: whiteRepository.GetById(board: currentBoard).Build(), 
            loggedUser: loggedUser, 
            unitOfWork: unitOfWork);
    }
}