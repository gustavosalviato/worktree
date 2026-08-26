using WorkTree.Communication.Responses;
using WorkTree.Exceptions.ExceptionsBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WorkTree.API.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is ExceptionBase exceptionBase)
        {
            context.HttpContext.Response.StatusCode = (int)exceptionBase.GetHttpStatusCode();
            context.Result = new ObjectResult(new ResponseErrorMessagesJson(exceptionBase.GetErrors()));
        }
        else
        {
            ThrowUnknownError(context);
        }
    }


    private void ThrowUnknownError(ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new ObjectResult(new ResponseErrorMessagesJson("Unknown error."));
    }
}