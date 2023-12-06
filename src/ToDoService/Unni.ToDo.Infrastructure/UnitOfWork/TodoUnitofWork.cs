using Unni.Todo.Application.Interfaces;
using Unni.Todo.Infrastructure.Context;

namespace Unni.Todo.Infrastructure.UnitOfWork
{
    public class TodoUnitOfWork : ITodoUnitOfWork
    {
        private readonly ToDoDBContext _context;

        public TodoUnitOfWork(ToDoDBContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
