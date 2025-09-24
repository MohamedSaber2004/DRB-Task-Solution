namespace DRB_Task.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
                await HandleNotFoundEndPointAsync(context);
                await HandleNotAllowedMethodAsync(context);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred:"); 
                await HandleOverAllExceptionsAsync(context, ex);
            }
        }

        private static async Task HandleOverAllExceptionsAsync(HttpContext context, Exception ex)
        {
            context.Response.StatusCode = ex switch
            {
                UnSupportedException => StatusCodes.Status415UnsupportedMediaType,
                BusinessLogicException => StatusCodes.Status422UnprocessableEntity,
                ConflictException => StatusCodes.Status409Conflict,
                NotAllowedException => StatusCodes.Status405MethodNotAllowed,
                NotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };

            context.Response.ContentType = "application/json";

            var response = new ErrorToReturn()
            {
                StatusCode = context.Response.StatusCode,
                ErrorMessage = ex.Message
            };

            await context.Response.WriteAsJsonAsync(response);
        }

        private static Task HandleNotFoundEndPointAsync(HttpContext context)
        {
            if (context.Response.StatusCode == StatusCodes.Status404NotFound)
                throw new EndPointNotFoundException(context.Request.Path);
            return Task.CompletedTask;
        }

        private static Task HandleNotAllowedMethodAsync(HttpContext context)
        {
            if (context.Response.StatusCode == StatusCodes.Status405MethodNotAllowed)
                throw new MethodNotAllowedException(context.Request.Method);
            return Task.CompletedTask;
        }
    }
}
