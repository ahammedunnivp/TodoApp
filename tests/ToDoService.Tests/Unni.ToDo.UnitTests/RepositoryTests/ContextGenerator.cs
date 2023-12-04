using Microsoft.EntityFrameworkCore;
using Unni.Todo.Infrastructure.Context;

namespace Unni.ToDo.Tests.RepositoryTests
{
    public static class ContextGenerator
    {
        public static ToDoDBContext Generate()
        {
            var optionBuilder = new DbContextOptionsBuilder<ToDoDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase");

            return new ToDoDBContext(optionBuilder.Options);
        }
    }
}
