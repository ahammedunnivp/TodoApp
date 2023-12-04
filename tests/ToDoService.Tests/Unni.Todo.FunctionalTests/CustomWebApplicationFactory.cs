using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Unni.Admin.Infrastructure.Data.Seeding;
using Unni.Todo.Infrastructure.Context;

namespace Unni.Todo.FunctionalTests;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        var host = builder.Build();
        host.Start();

        var serviceProvider = host.Services;

        using (var scope = serviceProvider.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<ToDoDBContext>();

            var logger = scopedServices
                .GetRequiredService<ILogger<CustomWebApplicationFactory<TProgram>>>();

            // Reset Sqlite database for each test run
            db.Database.EnsureDeleted();

            // Ensure the database is created.
            db.Database.EnsureCreated();

            try
            {
                if (!db.ToDoItems.Any())
                {
                    var dataseeder = new TodoDataSeeder(db);
                    dataseeder.SeedData();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding the " +
                                    "database with test messages. Error: {exceptionMessage}", ex.Message);
            }
        }

        return host;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder
            .ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<ToDoDBContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                string inMemoryCollectionName = Guid.NewGuid().ToString();

                services.AddDbContext<ToDoDBContext>(options =>
                {
                    options.UseInMemoryDatabase(inMemoryCollectionName);
                });
            });
    }
}