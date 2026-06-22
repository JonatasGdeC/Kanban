using System.Net;

namespace Kanban.Exception.ExceptionBase;

public class ErrorOnValidationException(List<string> errorsMessages) : ExceptionBase(message: string.Empty)
{
    public override int StatusCode => (int)HttpStatusCode.BadRequest;

    public override List<string> GetErrors()
    {
        return errorsMessages;
    }
}