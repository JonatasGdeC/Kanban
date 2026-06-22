using AutoMapper;
using Kanban.Communication.Dtos;
using Kanban.Communication.Responses.Column;
using Kanban.Domain.Repositories.Boad;
using Kanban.Domain.Repositories.Column;
using Kanban.Domain.Services.LoggedUser;
using Kanban.Exception;
using Kanban.Exception.ExceptionBase;

namespace Kanban.Application.UseCase.Column.GetAll;
using Domain.Entities;

public class GetAllColumnsUseCase(
    IColumnReadRepository readColumnRepository,
    IBoardReadRepository readBoardRepository,
    ILoggedUser loggedUser,
    IMapper mapper) : IGetAllColumnsUseCase
{
    public async Task<GetAllColumnsResponse> Execute(Guid boardId)
    {
        User user = await loggedUser.Get();
        await Validate(user: user, boardId: boardId);
        
        List<Column> columns = await readColumnRepository.GetAll(boardId: boardId, userId: user.Id);

        return new GetAllColumnsResponse
        {
            ListColumns = mapper.Map<List<ColumnDto>>(source: columns)
        };
    }

    private async Task Validate(User user, Guid boardId)
    {
       Board? board = await readBoardRepository.GetById(id: boardId, userId: user.Id);

       if (board == null)
       {
           throw new NotFoundException(message: ResourceErrorMessage.BOARD_NOT_FOUND);
       }
    }
}
