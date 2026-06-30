using AutoMapper;
using CommomTestsUtilies.Entities;
using CommomTestsUtilies.LoggedUser;
using CommomTestsUtilies.Mapper;
using CommomTestsUtilies.Repositories;
using CommomTestsUtilies.Repositories.SubTask;
using CommomTestsUtilies.Repositories.Task;
using CommomTestsUtilies.Requests;
using FluentAssertions;
using FluentAssertions.Specialized;
using Kanban.Application.UseCase.SubTask.Register;
using Kanban.Communication.Dtos;
using Kanban.Communication.Requests.SubTask;
using Kanban.Domain.Repositories;
using Kanban.Domain.Repositories.SubTask;
using Kanban.Domain.Repositories.Task;
using Kanban.Domain.Services.LoggedUser;
using Kanban.Exception;
using Kanban.Exception.ExceptionBase;

namespace UseCases.Tests.SubTask;
using Kanban.Domain.Entities;
using System.Threading.Tasks;

public class RegisterSubTaskUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        User user = UserBuilder.Build();
        TaskEntity task = TaskEntityBuilder.Build(userId: user.Id);
        RegisterSubTaskRequest request = RegisterSubTaskRequestBuilder.Build();
        RegisterSubTaskUseCase useCase = CreateUseCase(user: user, task: task);

        SubTaskDto response = await useCase.Execute(taskId: task.Id, request: request);

        response.Should().NotBeNull();
        response.Name.Should().NotBeNullOrWhiteSpace().And.Be(expected: request.Name);
    }

    [Fact]
    public async Task Error_Task_Not_Found()
    {
        User user = UserBuilder.Build();
        RegisterSubTaskRequest request = RegisterSubTaskRequestBuilder.Build();
        RegisterSubTaskUseCase useCase = CreateUseCase(user: user);

        Func<Task<SubTaskDto>> act = async () => await useCase.Execute(taskId: Guid.NewGuid(), request: request);

        ExceptionAssertions<NotFoundException>? result = await act.Should().ThrowAsync<NotFoundException>();
        result.Where(exceptionExpression: ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessage.TASK_NOT_FOUND));
    }

    private RegisterSubTaskUseCase CreateUseCase(User user, TaskEntity? task = null)
    {
        TaskReadRepositoryBuilder readTaskRepository = new();
        ISubTaskWriteRepository writeSubTaskRepository = new SubTaskWriteRepositoryBuilder().Build();
        IUnitOfWork unitOfWork = UnitOfWorkBuilder.Build();
        IMapper mapper = MapperBuilder.Build();
        ILoggedUser loggedUser = LoggedUserBuilder.Build(user: user);

        if (task != null)
        {
            readTaskRepository.GetById(task: task);
        }

        return new RegisterSubTaskUseCase(
            writeSubTaskRepository: writeSubTaskRepository,
            readTaskRepository: readTaskRepository.Build(),
            unitOfWork: unitOfWork,
            mapper: mapper,
            loggedUser: loggedUser);
    }
}
