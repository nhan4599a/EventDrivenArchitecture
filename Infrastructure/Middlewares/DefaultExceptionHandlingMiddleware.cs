using Global.Shared.Constants;
using Infrastructure.Constants;
using Infrastructure.Exceptions;
using Infrastructure.Middlewares.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Middlewares
{
    public class DefaultExceptionHandlingMiddleware : BaseMiddleware
    {
        protected ILogger<DefaultExceptionHandlingMiddleware> Logger { get; }

        public DefaultExceptionHandlingMiddleware(ILogger<DefaultExceptionHandlingMiddleware> logger, RequestDelegate next)
            : base(next)
        {
            Logger = logger;
        }

        protected override async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await Next(context);
            }
            catch (Exception e)
            {
                if (e is WebApplicationException applicationException)
                    await HandleApplicationException(context, applicationException);
                else
                    await HandleUnhandledException(context, e);
            }
        }

        private async Task HandleApplicationException(HttpContext context, WebApplicationException e)
        {
            context.Response.StatusCode = e.ResponseCode;
        }

        private async Task HandleUnhandledException(HttpContext context, Exception e)
        {
            context.Response.StatusCode = SharedConstants.HttpResponseCodes.INTERNAL_SERVER_ERROR;
        }
    }
}
