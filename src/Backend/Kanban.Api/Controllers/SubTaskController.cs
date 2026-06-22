using Kanban.Application.UseCase.SubTask.Delete;
using Kanban.Application.UseCase.SubTask.GetAll;
using Kanban.Application.UseCase.SubTask.Register;
using Kanban.Application.UseCase.SubTask.Update;
using Kanban.Communication.Dtos;
using Kanban.Communication.Requests.SubTask;
using Kanban.Communication.Responses;
using Kanban.Communication.Responses.SubTask;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kanban.Api.Controllers;

[Route(template: "[controller]")]
[ApiController]
[Authorize]
public class SubTaskController : ControllerBase
{
    [HttpPost]
    [Route(template: "{taskId}")]
    [ProducesResponseType(type: typeof(SubTaskDto), statusCode: StatusCodes.Status201Created)]
    [ProducesResponseType(type: typeof(ErrorResponse), statusCode: StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Register([FromServices] IRegisterSubTaskUseCase useCase, [FromRoute] Guid taskId, [FromBody] RegisterSubTaskRequest request)
    {
        SubTaskDto response = await useCase.Execute(taskId: taskId, request: request);
        return Created(uri: string.Empty, value: response);
    }

    [HttpGet]
    [Route(template: "{taskId}")]
    [ProducesResponseType(type: typeof(GetAllSubTasksResponse), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
    [ProducesResponseType(type: typeof(ErrorResponse), statusCode: StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAll([FromServices] IGetAllSubTasksUseCase useCase, [FromRoute] Guid taskId)
    {
        GetAllSubTasksResponse response = await useCase.Execute(taskId: taskId);
        if (response.ListSubTasks.Count > 0)
        {
            return Ok(value: response);
        }

        return NoContent();
    }

    [HttpDelete]
    [Route(template: "{id}")]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
    [ProducesResponseType(type: typeof(ErrorResponse), statusCode: StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromServices] IDeleteSubTaskUseCase useCase, [FromRoute] Guid id)
    {
        await useCase.Execute(id: id);
        return NoContent();
    }

    [HttpPut]
    [Route(template: "{id}")]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
    [ProducesResponseType(type: typeof(ErrorResponse), statusCode: StatusCodes.Status400BadRequest)]
    [ProducesResponseType(type: typeof(ErrorResponse), statusCode: StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromServices] IUpdateSubTaskUseCase useCase, [FromRoute] Guid id, [FromBody] UpdateSubTaskRequest request)
    {
        await useCase.Execute(id: id, request: request);
        return NoContent();
    }
}
