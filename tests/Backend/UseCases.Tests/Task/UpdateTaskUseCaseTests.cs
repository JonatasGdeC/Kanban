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
using Kanban.Application.UseCase.TaskEntity.Update;
using Kanban.Communication.Requests.Task;
using Kanban.Domain.Repositories;
using Kanban.Domain.Services.LoggedUser;
using Kanban.Exception;
using Kanban.Exception.ExceptionBase;

namespace UseCases.Tests.Task;
using Kanban.Domain.Entities;
using System.Threading.Tasks;

public class UpdateTaskUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        User user = UserBuilder.Build();
        TaskEntity task = TaskEntityBuilder.Build(userId: user.Id);
        Column column = ColumnBuilder.Build(userId: user.Id);
        UpdateTaskRequest request = UpdateTaskRequestBuilder.Build(columnId: column.Id);
        UpdateTaskUseCase useCase = CreateUseCase(user: user, currentTask: task, column: column);

        Func<Task> act = async () => await useCase.Execute(id: task.Id, request: request);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_Task_Not_Found()
    {
        User user = UserBuilder.Build();
        TaskEntity task = TaskEntityBuilder.Build();
        Column column = ColumnBuilder.Build(userId: user.Id);
        UpdateTaskRequest request = UpdateTaskRequestBuilder.Build(columnId: column.Id);
        UpdateTaskUseCase useCase = CreateUseCase(user: user, currentTask: task, column: column);

        Func<Task> act = async () => await useCase.Execute(id: Guid.NewGuid(), request: request);

        ExceptionAssertions<NotFoundException>? result = await act.Should().ThrowAsync<NotFoundException>();
        result.Where(exceptionExpression: ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessage.TASK_NOT_FOUND));
    }

    [Fact]
    public async Task Error_Column_Not_Found()
    {
        User user = UserBuilder.Build();
        TaskEntity task = TaskEntityBuilder.Build(userId: user.Id);
        Column column = ColumnBuilder.Build();
        UpdateTaskRequest request = UpdateTaskRequestBuilder.Build(columnId: column.Id);
        UpdateTaskUseCase useCase = CreateUseCase(user: user, currentTask: task, column: null);

        Func<Task> act = async () => await useCase.Execute(id: task.Id, request: request);

        ExceptionAssertions<NotFoundException>? result = await act.Should().ThrowAsync<NotFoundException>();
        result.Where(exceptionExpression: ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessage.COLUMN_NOT_FOUND));
    }

    [Fact]
    public async Task Error_Task_Already_In_Position()
    {
        User user = UserBuilder.Build();
        TaskEntity task = TaskEntityBuilder.Build(userId: user.Id);
        Column column = ColumnBuilder.Build(userId: user.Id);
        UpdateTaskRequest request = UpdateTaskRequestBuilder.Build(columnId: column.Id);
        UpdateTaskUseCase useCase = CreateUseCase(user: user, currentTask: task, column: column, existsTaskInPosition: true);

        Func<Task> act = async () => await useCase.Execute(id: task.Id, request: request);

        ExceptionAssertions<ErrorOnValidationException>? result = await act.Should().ThrowAsync<ErrorOnValidationException>();
        result.Where(exceptionExpression: ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessage.TASK_ALREADY_IN_POSITION));
    }

    private UpdateTaskUseCase CreateUseCase(User user, TaskEntity currentTask, Column? column, bool existsTaskInPosition = false)
    {
        TaskWriteRepositoryBuilder writeRepositoryBuilder = new TaskWriteRepositoryBuilder().GetById(task: currentTask);
        TaskReadRepositoryBuilder readRepositoryBuilder = new TaskReadRepositoryBuilder().ExistsTaskInPosition(exists: existsTaskInPosition);
        ColumnReadRepositoryBuilder readColumnRepositoryBuilder = new();
        ILoggedUser loggedUser = LoggedUserBuilder.Build(user: user);
        IMapper mapper = MapperBuilder.Build();
        IUnitOfWork unitOfWork = UnitOfWorkBuilder.Build();
        
        if (column != null)
        {
            readColumnRepositoryBuilder.GetById(column: column);
        }

        return new UpdateTaskUseCase(
            writeRepository: writeRepositoryBuilder.Build(),
            readRepository: readRepositoryBuilder.Build(),
            readColumnRepository: readColumnRepositoryBuilder.Build(),
            loggedUser: loggedUser,
            mapper: mapper,
            unitOfWork: unitOfWork);
    }
}
