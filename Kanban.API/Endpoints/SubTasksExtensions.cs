using Kanban.API.Request;
using Kanban.DB.Banco;
using Kanban.Shared.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace Kanban.API.Endpoints;

public static class SubTasksExtensions
{
  public static void AddEndPointSubTasks(this WebApplication app)
  {
    var group = app.MapGroup("/SubTasks").WithTags("TasksSub");

    group.MapGet("/{idTask}", ([FromServices] DAL<Subtasks> dal, int idTask) =>
    {
      var listFromSubTasks = dal.Listar().Where(subtasks => subtasks.TaskId == idTask);

      if (listFromSubTasks is null) return Results.NotFound();

      return Results.Ok(listFromSubTasks);
    });

    group.MapPost("/", ([FromServices] DAL<Subtasks> dal, [FromBody] SubTasksRequest subTasksRequest) =>
    {
      var subtasks = new Subtasks()
      {
        TaskId = subTasksRequest.IdTask,
        Title = subTasksRequest.Title,
        IsCompleted = subTasksRequest.IsCompleted
      };

      dal.Adicionar(subtasks);
      return Results.Ok();
    });

    group.MapDelete("/{id}", ([FromServices] DAL<Subtasks> dal, int id) =>
    {
      var subtask = dal.RecuperarPor(subtask => subtask.Id == id);

      if (subtask is null) return Results.NotFound();

      dal.Deletar(subtask);
      return Results.NoContent();
    });
  }
}
