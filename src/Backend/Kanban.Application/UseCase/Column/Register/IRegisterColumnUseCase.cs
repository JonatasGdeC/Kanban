using Kanban.Communication.Dtos;
using Kanban.Communication.Requests.Column;

namespace Kanban.Application.UseCase.Column.Register;

public interface IRegisterColumnUseCase
{
    Task<ColumnDto> Execute(Guid boardId, RegisterColumnRequest request);
}
