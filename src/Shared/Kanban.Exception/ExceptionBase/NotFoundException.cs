using System.Net;

namespace Kanban.Exception.ExceptionBase;

public class NotFoundException(string message) : ExceptionBase(message: message)
{
    public override int StatusCode => (int)HttpStatusCode.NotFound;

    public override List<string> GetErrors()
    {
        return [Message];
    }
}