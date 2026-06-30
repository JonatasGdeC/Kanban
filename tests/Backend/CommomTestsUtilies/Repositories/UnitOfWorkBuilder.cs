using Kanban.Domain.Repositories;
using Moq;

namespace CommomTestsUtilies.Repositories;

public static class UnitOfWorkBuilder
{
    public static IUnitOfWork Build()
    {
       Mock<IUnitOfWork> mock = new();
       return mock.Object;
    }
}