
using Kanban.Communication.Dtos;

namespace Kanban.App.UseState;

public class BoardUseState : UseStateOnChange
{
    private Dictionary<Guid, BoardDto> _listBoards = [];

    public void Set(BoardDto board)
    {
        _listBoards[key: board.Id] = board;
        Notify();
    }

    public void Set(List<BoardDto> boards)
    {
        _listBoards = boards.ToDictionary(keySelector: board => board.Id);
        Notify();
    }

    public void Clear()
    {
        _listBoards.Clear();
        Notify();
    }

    public void Remove(Guid itemId)
    {
        _listBoards.Remove(key: itemId);
        Notify();
    }


    public  IReadOnlyList<BoardDto> List() => _listBoards.Values.ToList();
}