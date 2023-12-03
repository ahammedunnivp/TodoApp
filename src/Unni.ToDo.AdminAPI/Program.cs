using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Unni.ToDo.Core.Interfaces;
using Unni.ToDo.Infrastructure.Data.Repositories;
using Unni.ToDo.Infrastructure.Data.UnitOfWork;
using AutoMapper;
using Unni.ToDo.Core.Services;
using Unni.ToDo.API.Mappers.Profiles;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AdminDbContext>(options =>
        options.UseSqlite("Data Source=Data\\Db\\admin.db"));

builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IAdminUnitOfWork, AdminUnitOfWork>();
builder.Services.AddScoped<IAdminService, AdminService>();

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new CategoryProfile());
});

builder.Services.AddAutoMapper(Assembly.GetEntryAssembly());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
