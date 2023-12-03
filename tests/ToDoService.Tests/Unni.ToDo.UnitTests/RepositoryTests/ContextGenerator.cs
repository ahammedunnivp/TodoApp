using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unni.ToDo.Infrastructure.Data.Repositories;

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
