using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Unni.Todo.Application.AutoMapperProfile;
using Unni.Todo.Application.Interfaces;
using Unni.Todo.Application.Services;
using Unni.Todo.Infrastructure.Context;
using Unni.Todo.Infrastructure.Repositories;
using Unni.Todo.Infrastructure.UnitOfWork;
using Unni.Todo.WebAPI.Filters;


namespace Unni.Todo.WebAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        //public void ConfigureLogging()
        //{
        //    Log.Logger = new LoggerConfiguration()
        //        .WriteTo.Console()
        //        .WriteTo.File("\\Logs\\log.txt", rollingInterval: RollingInterval.Day)
        //        .CreateLogger();
        //}

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {

                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        context.Response.ContentType = "application/json";

                        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                        var exception = exceptionHandlerPathFeature?.Error;

                        var response = new
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "An unexpected error occurred.",
                            Exception = exception?.Message
                        };

                        await context.Response.WriteAsJsonAsync(response);
                    });
                });
            }
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseRouting();
            app.UseResponseCaching();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add(new ValidateModelAttribute());
            });
            services.AddSingleton(Configuration);

            services.AddMemoryCache();
            services.AddResponseCaching();
            services.AddLogging(builder =>
            {
                builder.AddConsole();
            });
            services.AddDbContext<ToDoDBContext>(options =>
                    options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));


            services.AddScoped<ITodoRepository, TodoItemRepository>();
            services.AddScoped<ITodoUnitOfWork, TodoUnitOfWork>();
            services.AddScoped<ITodoService, TodoService>();
            services.AddAutoMapper(typeof(ToDoProfile));

            services.AddSwaggerGen();
            services.AddEndpointsApiExplorer();

        }
    }
}
