using Kanban.Communication.Responses;
using Kanban.Exception;
using Kanban.Exception.ExceptionBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Kanban.Api.Filter;

public class ExceptionFilter: IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is ExceptionBase)
        {
            HandleProjectException(context: context);
        }
        else
        {
            ThrowUnknowError(context: context);
        }
    }

    private void HandleProjectException(ExceptionContext context)
    {
        ExceptionBase? kanbanException = context.Exception as ExceptionBase;
        ErrorResponse errorResponse = new(errorMessages: kanbanException!.GetErrors());

        context.HttpContext.Response.StatusCode = kanbanException.StatusCode;
        context.Result = new ObjectResult(value: errorResponse);
    }

    private void ThrowUnknowError(ExceptionContext context)
    {
        // ErrorResponse errorResponse = new(errorMessage: ResourceErrorMessage.UNKNOWN_ERROR);
        ErrorResponse errorResponse = new(errorMessage: context.Exception.Message);
        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new ObjectResult(value: errorResponse);
    }
}