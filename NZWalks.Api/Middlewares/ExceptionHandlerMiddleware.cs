using System.Net;

namespace NZWalks.Api.Middlewares
{
    public class ExceptionHandlerMiddleware
    {

        public ILogger<ExceptionHandlerMiddleware> Logger { get; }
        public RequestDelegate Next { get; }
        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger,RequestDelegate next)
        {
            Logger = logger;
            Next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await  Next(httpContext);
            }
            catch(Exception ex)
            {
                var errorId = Guid.NewGuid();              
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";
                var error = new
                {
                    id = errorId,
                    ErrorMessage = "SomeThing webt wrong"
                };
                await httpContext.Response.WriteAsJsonAsync(error);
                
            }
        }
    }
}
