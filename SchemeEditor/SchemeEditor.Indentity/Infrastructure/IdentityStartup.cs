using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SchemeEditor.Identity.Abstractions;
using SchemeEditor.Identity.Settings;

namespace SchemeEditor.Identity.Infrastructure
{
	public static class IdentityStartup
	{
		public static void InitIdentity<TUser, TRole, TExecutionContext>(this IServiceCollection services, IConfiguration configuration)
			where TUser: class, IApplicationUser
			where TRole: class, IApplicationRole
			where TExecutionContext: class, IExecutionContext<TUser>
		{
			services.AddTransient<IUserStore<TUser>, UserStore<TUser, TRole>>();
			services.AddTransient<IRoleStore<TRole>, RoleStore<TUser, TRole>>();
			services.AddScoped<IExecutionContext<TUser>, TExecutionContext>();
			services.AddScoped<IPasswordHasher<TUser>, IdentityPasswordHasher<TUser>>();
			services.AddIdentity<TUser, TRole>()
				.AddRoleStore<RoleStore<TUser, TRole>>()
				.AddRoles<TRole>()
				.AddDefaultTokenProviders();

			services.AddAuthorization(options =>
			{
				BasicRoles.Roles.ToList().ForEach(x =>
				{
					options.AddPolicy(x.Key, builder =>
					{
						builder.RequireRole(x.Key);
					});
				});
			});
			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(cfg =>
			{
				cfg.RequireHttpsMetadata = false;
				cfg.SaveToken = true;
				cfg.TokenValidationParameters = new TokenValidationParameters
				{
					ValidIssuer = configuration["JwtIssuer"],
					ValidAudience = configuration["JwtIssuer"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtKey"])),
					LifetimeValidator = (before, expires, token, param) => expires > DateTime.UtcNow,
					ValidateLifetime = true
				};
				cfg.Events = new JwtBearerEvents
				{
					OnMessageReceived = context =>
					{
						var accessToken = context.Request.Query["access_token"];

						var path = context.HttpContext.Request.Path;
						if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/api/notifications")))
						{
							context.Token = accessToken;
						}
						return Task.CompletedTask;
					}
				};
			});
		}
	}
}
