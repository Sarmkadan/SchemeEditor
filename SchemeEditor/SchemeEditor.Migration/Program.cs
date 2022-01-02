using System;
using System.IO;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SchemeEditor.Migration.Migrations;

namespace SchemeEditor.Migration
{
    class Program
    {
        public static IConfiguration Configuration;
        
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            Configuration = builder
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            var serviceProvider = CreateServices();
            
            using (var scope = serviceProvider.CreateScope())
            {
	            var version = args.Length > 0 ? long.Parse(args[0]) : (long?) null;
                UpdateDatabase(scope.ServiceProvider, version);
            }
            
            Console.WriteLine("Процесс миграции базы данных завершен!");
        }
        
        private static IServiceProvider CreateServices()
        {
            var connectionStr = Configuration.GetConnectionString("SchemeEditor");
            return new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddPostgres()
                    .WithGlobalConnectionString(connectionStr)
                    .ScanIn(typeof(AddSchemeTable).Assembly).For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .BuildServiceProvider(false);
        }
        
        private static void UpdateDatabase(IServiceProvider serviceProvider, long? version)
        {
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
            if (version.HasValue)
            {
	            runner.MigrateDown(version.Value);
            }
            else
            {
	            runner.MigrateUp();
            }
        }
    }
}