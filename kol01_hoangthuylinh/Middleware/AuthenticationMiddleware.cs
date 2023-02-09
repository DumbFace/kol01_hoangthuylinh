using kol01_hoangthuylinh.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kol01_hoangthuylinh.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            var path = httpContext.Request.Path;
            if (path.Value.StartsWith("/admincp"))
            {
                if (httpContext.Request.Cookies[myCommon.hsCookieName] == null)
                {
                    httpContext.Response.Redirect("/dang-nhap");
                }
                else
                {
                    var user = MyAuthorize.GetLoggedUser(httpContext);
                    if (user == null)
                    {
                        httpContext.Response.Redirect("/dang-nhap");
                    }
                }

            }
            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class AutheticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseAutheticationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthenticationMiddleware>();
        }
    }
}
