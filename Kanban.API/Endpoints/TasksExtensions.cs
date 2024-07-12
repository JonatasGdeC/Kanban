using Kanban.API.Request;
using Kanban.API.Response;
using Kanban.DB.Banco;
using Kanban.Shared.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace Kanban.API.Endpoints;

public static class TasksExtensions
{
  public static void AddEndPointTasks(this WebApplication app)
  {
    var group = app.MapGroup("/Tasks").WithTags("Tasks");

    group.MapGet("/{idColumn}", ([FromServices] DAL<Tasks> dal, int idColumn) =>
    {
      var listFromTasks = dal.Listar().Where(task => task.ColumnId == idColumn);

      if (listFromTasks is null) return Results.NotFound();

      var listFromTasksResponse = EntityListToResponseList(listFromTasks);
      return Results.Ok(listFromTasksResponse);
    });

    group.MapPost("/", ([FromServices] DAL<Tasks> dal, [FromBody] TasksRequest tasksRequest) =>
    {
      var tasks = new Tasks()
      {
        ColumnId = tasksRequest.ColumnId,
        Title = tasksRequest.Title,
        Description = tasksRequest.Description,
        Position = tasksRequest.Position
      };

      dal.Adicionar(tasks);
      return Results.Ok();
    });

    group.MapDelete("/{id}", ([FromServices] DAL<Tasks> dal, int id) =>
    {
      var task = dal.RecuperarPor(a => a.Id == id);

      if (task is null) return Results.NotFound();

      dal.Deletar(task);
      return Results.NoContent();
    });
  }

  private static ICollection<TaskResponse> EntityListToResponseList(IEnumerable<Tasks> tasks)
  {
    return tasks.Select(a => EntityToResponse(a)).ToList();
  }

  private static TaskResponse EntityToResponse(Tasks tasks)
  {
    return new TaskResponse(tasks.Id,  tasks.Title!, tasks.Description!, tasks.DueDate, tasks.Position);
  }
}
