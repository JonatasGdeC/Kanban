using System.Net;

namespace Kanban.Exception.ExceptionBase;

public class BadRequestException(string message) : ExceptionBase(message: message)
{
    public override int StatusCode => (int)HttpStatusCode.BadRequest;

    public override List<string> GetErrors()
    {
        return [Message];
    }
}