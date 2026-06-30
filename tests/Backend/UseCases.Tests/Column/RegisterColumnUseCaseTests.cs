using AutoMapper;
using CommomTestsUtilies.Entities;
using CommomTestsUtilies.LoggedUser;
using CommomTestsUtilies.Mapper;
using CommomTestsUtilies.Repositories;
using CommomTestsUtilies.Repositories.Board;
using CommomTestsUtilies.Repositories.Column;
using CommomTestsUtilies.Requests;
using FluentAssertions;
using FluentAssertions.Specialized;
using Kanban.Application.UseCase.Column.Register;
using Kanban.Communication.Dtos;
using Kanban.Communication.Requests.Column;
using Kanban.Domain.Repositories;
using Kanban.Domain.Repositories.Boad;
using Kanban.Domain.Repositories.Column;
using Kanban.Domain.Services.LoggedUser;
using Kanban.Exception;
using Kanban.Exception.ExceptionBase;

namespace UseCases.Tests.Column;
using Kanban.Domain.Entities;
using System.Threading.Tasks;

public class RegisterColumnUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        User user = UserBuilder.Build();
        Board board = BoardBuilder.Build(userId: user.Id);
        RegisterColumnRequest request = RegisterColumnRequestBuilder.Build();
        RegisterColumnUseCase useCase = CreateUseCase(user: user, board: board);

        ColumnDto response = await useCase.Execute(boardId: board.Id, request: request);

        response.Should().NotBeNull();
        response.Name.Should().NotBeNullOrWhiteSpace().And.Be(expected: request.Name);
        response.Color.Should().NotBeNullOrWhiteSpace().And.Be(expected: request.Color);
    }

    [Fact]
    public async Task Error_Board_Not_Found()
    {
        User user = UserBuilder.Build();
        Board board = BoardBuilder.Build(userId: user.Id);
        RegisterColumnRequest request = RegisterColumnRequestBuilder.Build();
        RegisterColumnUseCase useCase = CreateUseCase(user: user);

        Func<Task<ColumnDto>> act = async () => await useCase.Execute(boardId: board.Id, request: request);

        ExceptionAssertions<NotFoundException>? result = await act.Should().ThrowAsync<NotFoundException>();
        result.Where(exceptionExpression: ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessage.BOARD_NOT_FOUND));
    }

    private RegisterColumnUseCase CreateUseCase(User user, Board? board = null)
    {

        BoardReadRepositoryBuilder readBoardRepository = new();
        IColumWriteRepository writeColumnRepository = new ColumnWriteRepositoryBuilder().Build();
        ColumnReadRepositoryBuilder readColumnRepository = new();
        IUnitOfWork unitOfWork = UnitOfWorkBuilder.Build();
        IMapper mapper = MapperBuilder.Build();
        ILoggedUser loggedUser = LoggedUserBuilder.Build(user: user);
        
        if (board != null)
        {
            readBoardRepository.GetById(board: board);
            readColumnRepository.GetAll(boardId: board.Id, userId: user.Id);
        }

        return new RegisterColumnUseCase(
            writeColumnRepository: writeColumnRepository,
            readColumnRepository: readColumnRepository.Build(),
            readBoardRepository: readBoardRepository.Build(),
            unitOfWork: unitOfWork,
            mapper: mapper,
            loggedUser: loggedUser);
    }
}
