using Unni.ToDo.Core.Interfaces;
using Unni.ToDo.Infrastructure.Data.Repositories;

namespace Unni.ToDo.Infrastructure.Data.UnitOfWork
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
