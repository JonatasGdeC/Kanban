using Kanban.Communication.Requests.Column;

namespace Kanban.Application.UseCase.Column.Update;

public interface IUpdateColumnUseCase
{
    Task Execute(Guid id, UpdateColumnRequest request);
}
