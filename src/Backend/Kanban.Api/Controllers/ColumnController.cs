using Kanban.Application.UseCase.Column.Delete;
using Kanban.Application.UseCase.Column.GetAll;
using Kanban.Application.UseCase.Column.GetById;
using Kanban.Application.UseCase.Column.Register;
using Kanban.Application.UseCase.Column.Update;
using Kanban.Communication.Dtos;
using Kanban.Communication.Requests.Column;
using Kanban.Communication.Responses;
using Kanban.Communication.Responses.Column;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kanban.Api.Controllers;

[Route(template: "[controller]")]
[ApiController]
[Authorize]
public class ColumnController : ControllerBase
{
    [HttpPost]
    [Route(template: "{boardId}")]
    [ProducesResponseType(type: typeof(ColumnDto), statusCode: StatusCodes.Status201Created)]
    public async Task<IActionResult> Register([FromServices] IRegisterColumnUseCase useCase, [FromRoute] Guid boardId, [FromBody] RegisterColumnRequest request)
    {
        ColumnDto response = await useCase.Execute(boardId: boardId, request: request);
        return Created(uri: string.Empty, value: response);
    }

    [HttpGet]
    [Route(template: "{boardId}")]
    [ProducesResponseType(type: typeof(GetAllColumnsResponse), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAll([FromServices] IGetAllColumnsUseCase useCase, [FromRoute] Guid boardId)
    {
        GetAllColumnsResponse response = await useCase.Execute(boardId: boardId);
        if (response.ListColumns.Count > 0)
        {
            return Ok(value: response);
        }

        return NoContent();
    }

    [HttpGet]
    [Route(template: "details/{id}")]
    [ProducesResponseType(type: typeof(GetColumnByIdResponse), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(type: typeof(ErrorResponse), statusCode: StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromServices] IGetColumnByIdUseCase useCase, [FromRoute] Guid id)
    {
        GetColumnByIdResponse response = await useCase.Execute(id: id);
        return Ok(value: response);
    }

    [HttpDelete]
    [Route(template: "{id}")]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
    [ProducesResponseType(type: typeof(ErrorResponse), statusCode: StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromServices] IDeleteColumnUseCase useCase, [FromRoute] Guid id)
    {
        await useCase.Execute(id: id);
        return NoContent();
    }

    [HttpPut]
    [Route(template: "{id}")]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
    [ProducesResponseType(type: typeof(ErrorResponse), statusCode: StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromServices] IUpdateColumnUseCase useCase, [FromRoute] Guid id, [FromBody] UpdateColumnRequest request)
    {
        await useCase.Execute(id: id, request: request);
        return NoContent();
    }
}
