using Microsoft.EntityFrameworkCore;
using Unni.ToDo.API.Filters;
using Unni.ToDo.Core.Interfaces;
using Unni.ToDo.Core.Services;
using Unni.ToDo.Infrastructure.Data.Repositories;
using Unni.ToDo.Infrastructure.Data.UnitOfWork;

namespace Unni.ToDo.API
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
                app.UseExceptionHandler();
            }
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseRouting();
            app.UseHttpsRedirection();
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
                //builder.ClearProviders();
                //builder.AddSerilog();
            });
            services.AddDbContext<ToDoDBContext>(options =>
                    options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));


            services.AddScoped<ITodoRepository, TodoItemRepository>();


            services.AddScoped<ITodoUnitOfWork, TodoUnitOfWork>();


            services.AddScoped<ITodoService, TodoService>();


            services.AddAutoMapper(typeof(Startup));

            services.AddSwaggerGen();
            services.AddEndpointsApiExplorer();

        }
    }
}
