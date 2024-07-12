using Kanban.API.Request;
using Kanban.API.Response;
using Kanban.DB.Banco;
using Kanban.Shared.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace Kanban.API.Endpoints;

public static class BoardsExtensions
{
  public static void AddEndPointsBoards(this WebApplication app)
  {
    var group = app.MapGroup("/Boards").WithTags("Boards");

    group.MapGet("/", ([FromServices] DAL<Boards> dal) =>
    {
      var listFromBoards = dal.Listar();
      if(listFromBoards is null)
      {
        return Results.NotFound();
      }

      var listFromBoardsResponse = EntityListToResponseList(listFromBoards);
      return Results.Ok(listFromBoardsResponse);
    });

    group.MapGet("/{id}", ([FromServices] DAL<Boards> dal, int id) =>
    {
      var board = dal.RecuperarPor(a => a.Id == id);
      if(board is null)
      {
        return Results.NotFound();
      }
      return Results.Ok(board);
    });

    group.MapPost("/",
    ([FromServices] DAL<Boards> dal, [FromServices] DAL<Columns> dalColumns,
      [FromBody] BoardsRequest boardsRequest) =>
    {
      var board = new Boards(boardsRequest.Name, boardsRequest.CreatedAt)
      {
        Name = boardsRequest.Name,
        CreatedAt = boardsRequest.CreatedAt,
      };

      dal.Adicionar(board);
      return Results.Ok();
    });

    group.MapDelete("/{id}", ([FromServices] DAL<Boards> dal, int id) =>
    {
      var board = dal.RecuperarPor(a => a.Id == id);
      if (board is null)
      {
        return Results.NotFound();
      }
      dal.Deletar(board);
      return Results.NoContent();
    });
  }

  private static ICollection<BoardsResponse> EntityListToResponseList(IEnumerable<Boards> boards)
  {
    return boards.Select(a => EntityToResponse(a)).ToList();
  }

  private static BoardsResponse EntityToResponse(Boards board)
  {
    return new BoardsResponse(board.Id, board.Name!, board.CreatedAt!);
  }
}
