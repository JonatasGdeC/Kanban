using Kanban.Domain.Repositories;
using Kanban.Domain.Repositories.Boad;
using Kanban.Domain.Services.LoggedUser;
using Kanban.Exception;
using Kanban.Exception.ExceptionBase;

namespace Kanban.Application.UseCase.Board.Delete;
using Domain.Entities;

public class DeleteBoardUseCase(IBoardWriteRepository writeRepository, ILoggedUser loggedUser, IUnitOfWork unitOfWork) : IDeleteBoardUseCase
{
    public async Task Execute(Guid id)
    {
        Board board = await GetValidatedBoard(id: id);
        writeRepository.Delete(board: board);
        await unitOfWork.Commit();
    }

    private async Task<Board> GetValidatedBoard(Guid id)
    {
        User user = await loggedUser.Get();
        Board? board = await writeRepository.GetById(id: id, userId: user.Id);

        if (board == null)
        {
            throw new NotFoundException(message: ResourceErrorMessage.BOARD_NOT_FOUND);
        }
        
        return board;
    }
}