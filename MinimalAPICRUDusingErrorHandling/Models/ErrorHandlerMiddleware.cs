namespace MinimalAPICRUDusingErrorHandling.Models
{
    public class ErrorHandlerMiddleware
    {
        // Field to store the next middleware in the pipeline
        private readonly RequestDelegate _next;

        // Constructor to initialize the middleware with the next delegate
        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            // Assign the next delegate to the field
            _next = next;
        }

    }
}
