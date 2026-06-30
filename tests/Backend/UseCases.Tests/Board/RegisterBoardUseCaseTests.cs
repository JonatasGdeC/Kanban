using AutoMapper;
using CommomTestsUtilies.Entities;
using CommomTestsUtilies.LoggedUser;
using CommomTestsUtilies.Mapper;
using CommomTestsUtilies.Repositories;
using CommomTestsUtilies.Repositories.Board;
using CommomTestsUtilies.Requests;
using FluentAssertions;
using FluentAssertions.Specialized;
using Kanban.Application.UseCase.Board.Register;
using Kanban.Communication.Dtos;
using Kanban.Communication.Requests.Board;
using Kanban.Domain.Repositories;
using Kanban.Domain.Repositories.Boad;
using Kanban.Domain.Services.LoggedUser;
using Kanban.Exception;
using Kanban.Exception.ExceptionBase;

namespace UseCases.Tests.Board;
using Kanban.Domain.Entities;
using System.Threading.Tasks;

public class RegisterBoardUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        User user = UserBuilder.Build();
        RegisterBoardUseCase useCase = CreateUseCase(user: user);
        RegisterBoardRequest request = RegisterBoardRequestBuilder.Build();

        BoardDto response = await useCase.Execute(request: request);
        
        response.Should().NotBeNull();
        response.Name.Should().NotBeNullOrWhiteSpace();
        response.Name.Should().NotBeEmpty().And.Be(expected: request.Name);
    }

    [Fact]
    public async Task Error_Board_Already_Exists()
    {
        RegisterBoardRequest request = RegisterBoardRequestBuilder.Build();
        User user = UserBuilder.Build();
        Board board = BoardBuilder.Build(boardName: request.Name, userId: user.Id);
        RegisterBoardUseCase useCase = CreateUseCase(user: user, board: board);

        Func<Task<BoardDto>> act = async () => await useCase.Execute(request: request);
    
        ExceptionAssertions<ErrorOnValidationException>? result = await act.Should().ThrowAsync<ErrorOnValidationException>();
        result.Where(exceptionExpression: ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessage.BOARD_ALREADY_EXISTS));
    }

    private RegisterBoardUseCase CreateUseCase(User user, Board? board = null)
    {
        IBoardWriteRepository whiteRepository = board != null ? new BoardWriteRepositoryBuilder().GetByTitle(board: board).Build() : new BoardWriteRepositoryBuilder().Build();
        IUnitOfWork unitOfWork = UnitOfWorkBuilder.Build();
        IMapper mapper = MapperBuilder.Build();
        ILoggedUser loggedUser = LoggedUserBuilder.Build(user: user);
        
        return new RegisterBoardUseCase(
            whiteRepository: whiteRepository, 
            unitOfWork: unitOfWork, 
            mapper: mapper,
            loggedUser: loggedUser);
    }
}