using System;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Ordering.API.Extensions
{
	public static class HostExtensions
	{
		public static IHost MigrateDatabase<TContext>
		(
			this IHost host,
			Action<TContext, IServiceProvider> seeder,
			int? retry = 0
			) where TContext: DbContext
		{
			var retryForAvailability = retry.Value;

			using (var scope = host.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				var logger = services.GetRequiredService<ILogger<TContext>>();
				var context = services.GetService<TContext>();

				try
				{
					logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext));

					InvokeSeeder(seeder, context, services);

					logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TContext));


	
				}
				catch (SqlException ex)
				{
					logger.LogError(ex, "An error occurred while migrating the database used on context.");
					if (retryForAvailability < 50)
					{
						retryForAvailability++;
						System.Threading.Thread.Sleep(2000);
						MigrateDatabase<TContext>(host, seeder, retryForAvailability);
					}
				}
			}

			return host;
		}

		private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> sender, 
			TContext context, 
			IServiceProvider services) where TContext : DbContext
		{
			context.Database.Migrate();
			sender(context, services);
		}
	}
}