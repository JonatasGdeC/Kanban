using System.ComponentModel.DataAnnotations;

namespace Kanban.API.Request;

public record SubTasksRequest([Required] int IdTask, [Required] string Title, [Required] bool IsCompleted);
