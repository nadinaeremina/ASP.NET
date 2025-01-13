namespace UserChallange.Api.Middleware
{
    // MiddlewareBase - базовый класс для middleware приложения
    public abstract class MiddlewareBase
    {
        protected readonly RequestDelegate _next;

        public MiddlewareBase(RequestDelegate next)
        {
            _next = next;
        }

        public abstract Task InvokeAsync(HttpContext context);
    }
}
