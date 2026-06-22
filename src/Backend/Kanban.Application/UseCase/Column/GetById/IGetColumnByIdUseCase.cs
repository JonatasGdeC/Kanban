using Kanban.Communication.Responses.Column;

namespace Kanban.Application.UseCase.Column.GetById;

public interface IGetColumnByIdUseCase
{
    Task<GetColumnByIdResponse> Execute(Guid id);
}
