using AutoMapper;
using CommomTestsUtilies.Entities;
using CommomTestsUtilies.LoggedUser;
using CommomTestsUtilies.Mapper;
using CommomTestsUtilies.Repositories;
using CommomTestsUtilies.Repositories.Column;
using CommomTestsUtilies.Repositories.Task;
using CommomTestsUtilies.Requests;
using FluentAssertions;
using FluentAssertions.Specialized;
using Kanban.Application.UseCase.TaskEntity.Register;
using Kanban.Communication.Dtos;
using Kanban.Communication.Requests.Task;
using Kanban.Domain.Repositories;
using Kanban.Domain.Repositories.Task;
using Kanban.Domain.Services.LoggedUser;
using Kanban.Exception;
using Kanban.Exception.ExceptionBase;

namespace UseCases.Tests.Task;
using Kanban.Domain.Entities;
using System.Threading.Tasks;

public class RegisterTaskUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        User user = UserBuilder.Build();
        Column column = ColumnBuilder.Build(userId: user.Id);
        RegisterTaskRequest request = RegisterTaskRequestBuilder.Build();
        RegisterTaskUseCase useCase = CreateUseCase(user: user, column: column);

        TaskDto response = await useCase.Execute(columnId: column.Id, request: request);

        response.Should().NotBeNull();
        response.Name.Should().NotBeNullOrWhiteSpace().And.Be(expected: request.Name);
        response.Description.Should().Be(expected: request.Description);
        response.ColumnId.Should().NotBeEmpty().And.Be(expected: column.Id);
    }

    [Fact]
    public async Task Error_Column_Not_Found()
    {
        User user = UserBuilder.Build();
        Column column = ColumnBuilder.Build(userId: user.Id);
        RegisterTaskRequest request = RegisterTaskRequestBuilder.Build();
        RegisterTaskUseCase useCase = CreateUseCase(user: user);

        Func<Task<TaskDto>> act = async () => await useCase.Execute(columnId: column.Id, request: request);

        ExceptionAssertions<NotFoundException>? result = await act.Should().ThrowAsync<NotFoundException>();
        result.Where(exceptionExpression: ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessage.COLUMN_NOT_FOUND));
    }

    private RegisterTaskUseCase CreateUseCase(User user, Column? column = null)
    {
        ColumnReadRepositoryBuilder readColumnRepository = new();
        ITaskWriteRepository writeTaskRepository = new TaskWriteRepositoryBuilder().Build();
        TaskReadRepositoryBuilder readTaskRepository = new();
        IUnitOfWork unitOfWork = UnitOfWorkBuilder.Build();
        IMapper mapper = MapperBuilder.Build();
        ILoggedUser loggedUser = LoggedUserBuilder.Build(user: user);

        if (column != null)
        {
            readColumnRepository.GetById(column: column);
            readTaskRepository.GetAll(columnId: column.Id, userId: user.Id);
        }

        return new RegisterTaskUseCase(
            writeTaskRepository: writeTaskRepository,
            readTaskRepository: readTaskRepository.Build(),
            readColumnRepository: readColumnRepository.Build(),
            unitOfWork: unitOfWork,
            mapper: mapper,
            loggedUser: loggedUser);
    }
}
