using Microsoft.AspNetCore.Builder;

namespace SchemeEditor.API.Middlewares
{
	public static class MiddlewaresHelper
	{
		public static IApplicationBuilder UseTokenRefresh(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<IdentityMiddleware>();
		}
		public static IApplicationBuilder UseUserDetection(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<DetectUserMiddleware>();
		}
		public static IApplicationBuilder UseBlockedUserFilter(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<BlockedUserFilterMiddleware>();
		}
	}
}
