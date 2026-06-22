namespace Kanban.Communication.Responses;

public record ErrorResponse
{
    public List<string> ErrorMessages { get; init; }

    public ErrorResponse(string errorMessage)
    {
        ErrorMessages = [errorMessage];
    }

    public ErrorResponse(List<string> errorMessages)
    {
        ErrorMessages = errorMessages;
    }
}