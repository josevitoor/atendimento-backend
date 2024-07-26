using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AtendimentoBackend.Filters;
public class ApiExceptionFilter : IExceptionFilter
{
    private readonly ILogger<ApiExceptionFilter> _logger;

    public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        if (context.Exception is ArgumentException argEx)
        {
            // Log the specific error
            _logger.LogWarning(context.Exception, "Handled ArgumentException: Status Code 400");

                context.Result = new BadRequestObjectResult(new { message = argEx.Message });
                context.ExceptionHandled = true;
            }
            else
            {
            // Log the generic error
            _logger.LogError(context.Exception, "Ocorreu um exceção não tratada: Status Code 500");

            context.Result = new ObjectResult(new { message = "Ocorreu um problema ao tratar a sua solicitação: Status Code 500" })
            {
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            context.ExceptionHandled = true;
        }
    }
}