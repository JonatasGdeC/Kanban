using Kanban.API.Request;
using Kanban.API.Response;
using Kanban.DB.Banco;
using Kanban.Shared.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace Kanban.API.Endpoints;

public static class ColumnsExtensions
{
  public static void AddEndPointColumns(this WebApplication app)
  {
    var group = app.MapGroup("/Columns").WithTags("Columns");

    group.MapGet("/{idBoard}", ([FromServices] DAL<Columns> dal, int idBoard) =>
    {
      var listFromColumns = dal.Listar().Where(column => column.BoardId == idBoard);

      if (listFromColumns is null) return Results.NotFound();

      var listFromColumnsResponse = EntityListToResponseList(listFromColumns);
      return Results.Ok(listFromColumnsResponse);
    });

    group.MapPost("/", ([FromServices] DAL<Columns> dal, [FromBody] ColumnsRequest columnsRequest) =>
    {
      var column = new Columns()
      {
        BoardId = columnsRequest.BoardId,
        Name = columnsRequest.Name,
        Position = columnsRequest.Position
      };

      dal.Adicionar(column);
      return Results.Ok();
    });

    group.MapDelete("/{id}", ([FromServices] DAL<Columns> dal, int id) =>
    {
      var column = dal.RecuperarPor(a => a.Id == id);
      if (column is null)
      {
        return Results.NotFound();
      }
      dal.Deletar(column);
      return Results.NoContent();
    });
  }

  private static ICollection<ColumnsResponse> EntityListToResponseList(IEnumerable<Columns> columns)
  {
    return columns.Select(a => EntityToResponse(a)).ToList();
  }

  private static ColumnsResponse EntityToResponse(Columns columns)
  {
    return new ColumnsResponse(columns.Id, columns.Name!, columns.Position!);
  }
}
