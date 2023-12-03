using Unni.Admin.Domain.Interfaces;
using Unni.Admin.Infrastructure.Context;

namespace Unni.Admin.Infrastructure.UnitOfWork
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