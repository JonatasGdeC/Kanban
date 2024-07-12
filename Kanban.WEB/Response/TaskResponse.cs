namespace Kanban.WEB.Response;

public record TaskResponse(int Id, string Title, string Description, DateTime DueDate, int Position);
