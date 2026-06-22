using Kanban.Application.UseCase.Board.Delete;
using Kanban.Application.UseCase.Board.GetAll;
using Kanban.Application.UseCase.Board.GetById;
using Kanban.Application.UseCase.Board.Register;
using Kanban.Application.UseCase.Board.Update;
using Kanban.Communication.Dtos;
using Kanban.Communication.Requests.Board;
using Kanban.Communication.Responses;
using Kanban.Communication.Responses.Board;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kanban.Api.Controllers;

[Route(template: "[controller]")]
[ApiController]
[Authorize]
public class BoardController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(type: typeof(BoardDto), statusCode: StatusCodes.Status201Created)]
    [ProducesResponseType(type: typeof(ErrorResponse), statusCode: StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromServices] IRegisterBoardUseCase useCase, [FromBody] RegisterBoardRequest request)
    {
        BoardDto response = await useCase.Execute(request: request);
        return Created(uri: string.Empty, value: response);
    }
    
    [HttpGet]
    [ProducesResponseType(type: typeof(GetAllBoardsResponse), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAllCategories([FromServices] IGetAllBoardsUseCase useCase)
    {
        GetAllBoardsResponse response = await useCase.Execute();
        if (response.ListBoards.Count > 0)
        {
            return Ok(value: response);
        }
    
        return NoContent();
    }
    
    [HttpGet]
    [Route(template: "{id}")]
    [ProducesResponseType(type: typeof(GetBoardByIdResponse), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(type: typeof(ErrorResponse), statusCode: StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCategoryById([FromServices] IGetBoardByIdUseCase useCase, [FromRoute] Guid id)
    {
        GetBoardByIdResponse response = await useCase.Execute(id: id);
        return Ok(value: response);
    }
    
    [HttpDelete]
    [Route(template: "{id}")]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
    [ProducesResponseType(type: typeof(ErrorResponse), statusCode: StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromServices] IDeleteBoardUseCase useCase, [FromRoute] Guid id)
    {
        await useCase.Execute(id: id);
        return NoContent();
    }
    
    [HttpPut]
    [Route(template: "{id}")]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
    [ProducesResponseType(type: typeof(ErrorResponse), statusCode: StatusCodes.Status400BadRequest)]
    [ProducesResponseType(type: typeof(ErrorResponse), statusCode: StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromServices] IUpdateBoardUseCase useCase, [FromRoute] Guid id, [FromBody] RegisterBoardRequest request)
    {
        await useCase.Execute(id: id, request: request);
        return NoContent();
    }
}