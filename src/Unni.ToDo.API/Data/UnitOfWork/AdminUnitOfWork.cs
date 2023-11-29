using Unni.ToDo.API.Data.Repositories;

namespace Unni.ToDo.API.Data.UnitOfWork
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