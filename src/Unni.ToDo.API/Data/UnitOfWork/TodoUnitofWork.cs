using Unni.ToDo.API.Data.Repositories;
using Unni.ToDo.Common.Interfaces;

namespace Unni.ToDo.API.Data.UnitOfWork
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
