using AutoMapper;
using CommomTestsUtilies.Entities;
using CommomTestsUtilies.LoggedUser;
using CommomTestsUtilies.Mapper;
using CommomTestsUtilies.Repositories;
using CommomTestsUtilies.Repositories.Column;
using CommomTestsUtilies.Requests;
using FluentAssertions;
using FluentAssertions.Specialized;
using Kanban.Application.UseCase.Column.Update;
using Kanban.Communication.Requests.Column;
using Kanban.Domain.Repositories;
using Kanban.Domain.Repositories.Column;
using Kanban.Domain.Services.LoggedUser;
using Kanban.Exception;
using Kanban.Exception.ExceptionBase;

namespace UseCases.Tests.Column;
using Kanban.Domain.Entities;
using System.Threading.Tasks;

public class UpdateColumnUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        User user = UserBuilder.Build();
        Column column = ColumnBuilder.Build(userId: user.Id);
        UpdateColumnRequest request = UpdateColumnRequestBuilder.Build();
        UpdateColumnUseCase useCase = CreateUseCase(user: user, currentColumn: column);

        Func<Task> act = async () => await useCase.Execute(id: column.Id, request: request);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_Column_Not_Found()
    {
        User user = UserBuilder.Build();
        Column column = ColumnBuilder.Build();
        UpdateColumnRequest request = UpdateColumnRequestBuilder.Build();
        UpdateColumnUseCase useCase = CreateUseCase(user: user, currentColumn: column);

        Func<Task> act = async () => await useCase.Execute(id: Guid.NewGuid(), request: request);

        ExceptionAssertions<NotFoundException>? result = await act.Should().ThrowAsync<NotFoundException>();
        result.Where(exceptionExpression: ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessage.COLUMN_NOT_FOUND));
    }

    private UpdateColumnUseCase CreateUseCase(User user, Column currentColumn)
    {
        ILoggedUser loggedUser = LoggedUserBuilder.Build(user: user);
        IUnitOfWork unitOfWork = UnitOfWorkBuilder.Build();
        IMapper mapper = MapperBuilder.Build();

        ColumnWriteRepositoryBuilder writeRepositoryBuilder = new ColumnWriteRepositoryBuilder().GetById(column: currentColumn);

        return new UpdateColumnUseCase(
            writeRepository: writeRepositoryBuilder.Build(),
            loggedUser: loggedUser,
            mapper: mapper,
            unitOfWork: unitOfWork);
    }
}
