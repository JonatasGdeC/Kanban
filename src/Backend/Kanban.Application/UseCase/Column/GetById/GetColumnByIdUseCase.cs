using AutoMapper;
using Kanban.Communication.Dtos;
using Kanban.Communication.Responses.Column;
using Kanban.Domain.Repositories.Column;
using Kanban.Domain.Services.LoggedUser;
using Kanban.Exception;
using Kanban.Exception.ExceptionBase;

namespace Kanban.Application.UseCase.Column.GetById;
using Domain.Entities;

public class GetColumnByIdUseCase(
    IColumnReadRepository readRepository,
    ILoggedUser loggedUser,
    IMapper mapper) : IGetColumnByIdUseCase
{
    public async Task<GetColumnByIdResponse> Execute(Guid id)
    {
        Column column = await GetValidatedColumn(id: id);

        return new GetColumnByIdResponse
        {
            Column = mapper.Map<ColumnDto>(source: column),
            Tasks = mapper.Map<List<TaskDto>>(source: column.Tasks)
        };
    }

    private async Task<Column> GetValidatedColumn(Guid id)
    {
        User user = await loggedUser.Get();
        Column? column = await readRepository.GetById(id: id, userId: user.Id);

        if (column == null)
        {
            throw new NotFoundException(message: ResourceErrorMessage.COLUMN_NOT_FOUND);   
        }

        return column;
    }
}
