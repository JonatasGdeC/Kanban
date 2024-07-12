namespace Kanban.API.Response;

public record TaskResponse(int Id, string Title, string Description, DateTime DueDate, int Position);
