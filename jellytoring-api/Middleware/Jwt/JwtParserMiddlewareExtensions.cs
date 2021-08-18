using Microsoft.AspNetCore.Builder;
using WebApi.Middleware;

namespace jellytoring_api.Middleware.Jwt
{
    public static class JwtParserMiddlewareExtensions
    {
        public static IApplicationBuilder UseJwtParser(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JwtParserMiddleware>();
        }
    }
}
