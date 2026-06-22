using AutoMapper;
using Kanban.Communication.Dtos;
using Kanban.Communication.Responses.Board;
using Kanban.Domain.Repositories.Boad;
using Kanban.Domain.Services.LoggedUser;

namespace Kanban.Application.UseCase.Board.GetAll;
using Domain.Entities;

public class GetAllBoardsUseCase(
    IBoardReadRepository readRepository, 
    ILoggedUser loggedUser, 
    IMapper mapper) : IGetAllBoardsUseCase
{
    public async Task<GetAllBoardsResponse> Execute()
    {
        User user = await loggedUser.Get();
        List<Board> response = await readRepository.GetAll(userId: user.Id);
        
        return new GetAllBoardsResponse
        {
            ListBoards = mapper.Map<List<BoardDto>>(source: response)
        };
    }
}