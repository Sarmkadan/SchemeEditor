using System;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SchemeEditor.API.Middlewares;
using SchemeEditor.Application.Hubs;
using SchemeEditor.Application.Infrastructure;
using SchemeEditor.Application.Services;
using SchemeEditor.DAL.Services;
using SchemeEditor.Domain.Models;
using SchemeEditor.Identity.Infrastructure;
using SchemeEditor.Notifications.Infrastructure;

namespace SchemeEditor.API
{
	public class Startup
	{
		public Startup(IConfiguration configuration) => Configuration = configuration;
		public IConfiguration Configuration { get; }
		public IContainer ApplicationContainer { get; private set; }

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			//cors
			services.AddCors(opt => opt.AddPolicy("CorsPolicy", build =>
			{
				build.SetIsOriginAllowed(host => true)
					.AllowAnyHeader()
					.AllowAnyMethod()
					.AllowCredentials()
					.WithExposedHeaders("JwtToken");
			}));
			LinqToDB.Common.Configuration.Linq.AllowMultipleQuery = true;
			services.InitIdentity<User, Role, ExecutionContext>(Configuration);
			services.RegisterNotifications();
			services.AddResponseCompression();
			services.AddMvc()
				.SetCompatibilityVersion(CompatibilityVersion.Version_2_2);


			services.AddSignalR();
			services.AddSingleton<IUserIdProvider, NameUserIdProvider>();

			services.AddSwaggerDocument(s =>
			{
				s.Title = "Scheme Editor";
				s.Version = "v1";
			});

			// autofac
			var builder = new ContainerBuilder();
			builder.Populate(services);
			var applicationAssembly = Assembly.GetAssembly(typeof(SchemeService));
			builder.RegisterAssemblyTypes(applicationAssembly)
				.Where(t => t.Name.EndsWith("Service"))
				.AsImplementedInterfaces();
			builder.RegisterAssemblyTypes(applicationAssembly)
				.Where(t => t.Name.EndsWith("Provider"))
				.AsImplementedInterfaces();
			var dalAssembly = Assembly.GetAssembly(typeof(ConnectionService));
			builder.RegisterAssemblyTypes(dalAssembly)
				.Where(t => t.Name.EndsWith("Service"))
				.AsImplementedInterfaces();
			builder.RegisterAssemblyTypes(dalAssembly)
				.Where(t => t.Name.EndsWith("Repository"))
				.AsImplementedInterfaces();
			ApplicationContainer = builder.Build();
			return new AutofacServiceProvider(ApplicationContainer);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseCors("CorsPolicy")
				.UseAuthentication()
				.UseSignalR(routes =>
				{
					routes.MapHub<MessagesHub>("/api/notifications");
				})
				.UseResponseCompression()
				.UseStaticFiles()
				.UseOpenApi()
				.UseSwaggerUi3()
				.UseUserDetection()
				.UseBlockedUserFilter()
				.UseTokenRefresh()
				.UseMvc();
		}
	}
}
