using Unni.ToDo.UI.Components;

namespace Unni.ToDo.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var adminServiceUrl = Environment.GetEnvironmentVariable("AdminServiceUrl"); 

            var todoServiceUrl = Environment.GetEnvironmentVariable("TodoServiceUrl");

            builder.Services.AddHttpClient("AdminService", client =>
            {
                client.BaseAddress = new Uri(adminServiceUrl);
            });

            builder.Services.AddHttpClient("TodoService", client =>
            {
                client.BaseAddress = new Uri(todoServiceUrl);
            });

            // Add services to the container.
            builder.Services.AddAntiforgery();
            builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();
            builder.Services.AddHttpClient();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
