using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Infrastructure.Middlewares.Abstraction
{
    public abstract class BaseMiddleware
    {
        protected RequestDelegate Next { get; }

        protected BaseMiddleware(RequestDelegate next)
        {
            Next = next;
        }

        protected abstract Task InvokeAsync(HttpContext context);
    }
}
