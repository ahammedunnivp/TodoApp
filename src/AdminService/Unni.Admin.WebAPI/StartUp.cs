using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;
using Unni.Admin.Application.AutoMapperProfile;
using Unni.Admin.Application.Interfaces;
using Unni.Admin.Application.Services;
using Unni.Admin.Domain.Interfaces;
using Unni.Admin.Infrastructure.Context;
using Unni.Admin.Infrastructure.Repositories;
using Unni.Admin.Infrastructure.UnitOfWork;

namespace Unni.AdminAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddDbContext<AdminDbContext>(options =>
                    options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<IAdminUnitOfWork, AdminUnitOfWork>();
            services.AddScoped<IAdminService, AdminService>();

            services.AddAutoMapper(typeof(CategoryProfile));

            services.AddSwaggerGen();
            services.AddEndpointsApiExplorer();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {

                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
                app.UseHttpsRedirection();
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
    }
}
