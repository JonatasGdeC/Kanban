using System.Net;

namespace Kanban.Adapter.Exceptions;

public sealed class ApiException(HttpStatusCode statusCode, IReadOnlyList<string> errorMessages) : Exception(message: string.Join(separator: Environment.NewLine, values: errorMessages))
{
    public HttpStatusCode StatusCode { get; } = statusCode;
    public IReadOnlyList<string> ErrorMessages { get; } = errorMessages;
}
