using Kanban.Application.UseCase.TaskEntity.Delete;
using Kanban.Application.UseCase.TaskEntity.GetAll;
using Kanban.Application.UseCase.TaskEntity.GetById;
using Kanban.Application.UseCase.TaskEntity.Register;
using Kanban.Application.UseCase.TaskEntity.Update;
using Kanban.Communication.Dtos;
using Kanban.Communication.Requests.Task;
using Kanban.Communication.Responses;
using Kanban.Communication.Responses.Task;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kanban.Api.Controllers;

[Route(template: "[controller]")]
[ApiController]
[Authorize]
public class TaskController : ControllerBase
{
    [HttpPost]
    [Route(template: "{columnId}")]
    [ProducesResponseType(type: typeof(TaskDto), statusCode: StatusCodes.Status201Created)]
    [ProducesResponseType(type: typeof(ErrorResponse), statusCode: StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Register([FromServices] IRegisterTaskUseCase useCase, [FromRoute] Guid columnId, [FromBody] RegisterTaskRequest request)
    {
        TaskDto response = await useCase.Execute(columnId: columnId, request: request);
        return Created(uri: string.Empty, value: response);
    }

    [HttpGet]
    [Route(template: "{columnId}")]
    [ProducesResponseType(type: typeof(GetAllTasksResponse), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
    [ProducesResponseType(type: typeof(ErrorResponse), statusCode: StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAll([FromServices] IGetAllTasksUseCase useCase, [FromRoute] Guid columnId)
    {
        GetAllTasksResponse response = await useCase.Execute(columnId: columnId);
        if (response.ListTasks.Count > 0)
        {
            return Ok(value: response);
        }

        return NoContent();
    }

    [HttpGet]
    [Route(template: "details/{id}")]
    [ProducesResponseType(type: typeof(GetTaskByIdResponse), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(type: typeof(ErrorResponse), statusCode: StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromServices] IGetTaskByIdUseCase useCase, [FromRoute] Guid id)
    {
        GetTaskByIdResponse response = await useCase.Execute(id: id);
        return Ok(value: response);
    }

    [HttpDelete]
    [Route(template: "{id}")]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
    [ProducesResponseType(type: typeof(ErrorResponse), statusCode: StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromServices] IDeleteTaskUseCase useCase, [FromRoute] Guid id)
    {
        await useCase.Execute(id: id);
        return NoContent();
    }

    [HttpPut]
    [Route(template: "{id}")]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
    [ProducesResponseType(type: typeof(ErrorResponse), statusCode: StatusCodes.Status400BadRequest)]
    [ProducesResponseType(type: typeof(ErrorResponse), statusCode: StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromServices] IUpdateTaskUseCase useCase, [FromRoute] Guid id, [FromBody] UpdateTaskRequest request)
    {
        await useCase.Execute(id: id, request: request);
        return NoContent();
    }
}
