using AutoMapper;
using Kanban.Communication.Dtos;
using Kanban.Communication.Responses.Board;
using Kanban.Domain.Repositories.Boad;
using Kanban.Domain.Services.LoggedUser;
using Kanban.Exception;
using Kanban.Exception.ExceptionBase;

namespace Kanban.Application.UseCase.Board.GetById;
using Domain.Entities;

public class GetBoardByIdUseCase(
    IBoardReadRepository readRepository,
    ILoggedUser loggedUser,
    IMapper mapper) : IGetBoardByIdUseCase
{
    public async Task<GetBoardByIdResponse> Execute(Guid id)
    {
        Board board = await GetValidatedBoard(id: id);

        return new GetBoardByIdResponse
        {
            Board = mapper.Map<BoardDto>(source: board),
            Columns = mapper.Map<List<ColumnDto>>(source: board.Columns)
        };
    }

    private async Task<Board> GetValidatedBoard(Guid id)
    {
        User user = await loggedUser.Get();
        Board? board = await readRepository.GetById(id: id, userId: user.Id);

        if (board == null)
        {
            throw new NotFoundException(message: ResourceErrorMessage.BOARD_NOT_FOUND);
        }
        
        return board;
    }
}