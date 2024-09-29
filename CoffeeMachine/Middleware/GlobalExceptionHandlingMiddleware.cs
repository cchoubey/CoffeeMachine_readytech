using CoffeeMachine.AppLogic.CustomExceptions;
using System.Net;
using System.Text.Json;

namespace CoffeeMachine.Middleware
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;
        public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                await HandleExceptionAsyc(context, ex);
            }
        }

        private async Task HandleExceptionAsyc(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            var responseModel = new ResponseModel();

            switch (exception)
            {
                case TeaPotException:
                    var teapotException = exception as TeaPotException;
                    response.StatusCode = teapotException.StatuCode;
                    responseModel = null;
                    break;
                case ServiceUnavailableException:
                    var serviceUnavailableException = exception as ServiceUnavailableException;
                    response.StatusCode = serviceUnavailableException.StatuCode;
                    responseModel = null;
                    break;
                case ApplicationException:
                case ArgumentNullException:
                    responseModel.responseCode = (int)HttpStatusCode.BadRequest;
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    responseModel.responseMessage = exception.Message;
                    break;
                default:
                    responseModel.responseCode = (int)HttpStatusCode.InternalServerError;
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    responseModel.responseMessage = exception.Message;
                    break;
            }
            if (responseModel != null)
            {
                var result = JsonSerializer.Serialize(responseModel);
                await context.Response.WriteAsync(result);
            }
        }
    }
}
