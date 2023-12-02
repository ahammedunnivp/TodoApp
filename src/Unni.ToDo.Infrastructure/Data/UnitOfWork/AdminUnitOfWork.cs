using Unni.ToDo.Core.Interfaces;
using Unni.ToDo.Infrastructure.Data.Repositories;

namespace Unni.ToDo.Infrastructure.Data.UnitOfWork
{
    public class AdminUnitOfWork : IAdminUnitOfWork
    {
        private readonly AdminDbContext _context;

        public AdminUnitOfWork(AdminDbContext context)
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