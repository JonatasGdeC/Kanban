namespace Kanban.Domain.Security.Tokens;

public interface ITokenProvider
{
    string TokenOnRequest();
}