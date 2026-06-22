using System.Net;

namespace Kanban.Exception.ExceptionBase;

public class InvalidLoginException() : ExceptionBase(message: ResourceErrorMessage.INVALID_LOGIN)
{
    public override int StatusCode => (int)HttpStatusCode.Unauthorized;
    
    public override List<string> GetErrors()
    {
        return [Message];
    }
}