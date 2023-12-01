using Microsoft.EntityFrameworkCore;
using Unni.ToDo.API.Data.Repositories;
using Unni.ToDo.API.Data.UnitOfWork;
using Unni.ToDo.API.Filters;
using Unni.ToDo.API.Services;
using Unni.ToDo.Common.Interfaces;

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
            services.AddDbContext<AdminDbContext>(options =>
                    options.UseSqlite(Configuration.GetConnectionString("AdminConnection")));


            services.AddScoped<ITodoRepository, TodoItemRepository>();
            services.AddScoped<IAdminRepository, AdminRepository>();

            services.AddScoped<ITodoUnitOfWork, TodoUnitOfWork>();
            services.AddScoped<IAdminUnitOfWork, AdminUnitOfWork>();

            services.AddScoped<ITodoService, TodoService>();
            services.AddScoped<IAdminService, AdminService>();

            services.AddAutoMapper(typeof(Startup));

            services.AddSwaggerGen();
            services.AddEndpointsApiExplorer();

        }
    }
}
