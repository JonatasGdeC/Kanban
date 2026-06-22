using Kanban.Communication.Responses.Column;

namespace Kanban.Application.UseCase.Column.GetAll;

public interface IGetAllColumnsUseCase
{
    Task<GetAllColumnsResponse> Execute(Guid boardId);
}
