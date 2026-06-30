using AutoMapper;
using CommomTestsUtilies.Entities;
using CommomTestsUtilies.LoggedUser;
using CommomTestsUtilies.Mapper;
using CommomTestsUtilies.Repositories;
using CommomTestsUtilies.Repositories.SubTask;
using CommomTestsUtilies.Requests;
using FluentAssertions;
using FluentAssertions.Specialized;
using Kanban.Application.UseCase.SubTask.Update;
using Kanban.Communication.Requests.SubTask;
using Kanban.Domain.Repositories;
using Kanban.Domain.Services.LoggedUser;
using Kanban.Exception;
using Kanban.Exception.ExceptionBase;

namespace UseCases.Tests.SubTask;
using Kanban.Domain.Entities;
using System.Threading.Tasks;

public class UpdateSubTaskUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        User user = UserBuilder.Build();
        SubTask subTask = SubTaskBuilder.Build(userId: user.Id);
        UpdateSubTaskRequest request = UpdateSubTaskRequestBuilder.Build();
        UpdateSubTaskUseCase useCase = CreateUseCase(user: user, currentSubTask: subTask);

        Func<Task> act = async () => await useCase.Execute(id: subTask.Id, request: request);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_SubTask_Not_Found()
    {
        User user = UserBuilder.Build();
        SubTask subTask = SubTaskBuilder.Build();
        UpdateSubTaskRequest request = UpdateSubTaskRequestBuilder.Build();
        UpdateSubTaskUseCase useCase = CreateUseCase(user: user, currentSubTask: subTask);

        Func<Task> act = async () => await useCase.Execute(id: Guid.NewGuid(), request: request);

        ExceptionAssertions<NotFoundException>? result = await act.Should().ThrowAsync<NotFoundException>();
        result.Where(exceptionExpression: ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessage.SUBTASK_NOT_FOUND));
    }

    private UpdateSubTaskUseCase CreateUseCase(User user, SubTask currentSubTask)
    {
        SubTaskWriteRepositoryBuilder writeRepositoryBuilder = new SubTaskWriteRepositoryBuilder().GetById(subTask: currentSubTask);
        ILoggedUser loggedUser = LoggedUserBuilder.Build(user: user);
        IMapper mapper = MapperBuilder.Build();
        IUnitOfWork unitOfWork = UnitOfWorkBuilder.Build();

        return new UpdateSubTaskUseCase(
            writeRepository: writeRepositoryBuilder.Build(),
            loggedUser: loggedUser,
            mapper: mapper,
            unitOfWork: unitOfWork);
    }
}
