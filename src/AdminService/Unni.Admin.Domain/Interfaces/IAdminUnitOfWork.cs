namespace Unni.Admin.Domain.Interfaces
{
    public interface IAdminUnitOfWork : IDisposable
    {
        void SaveChanges();
    }
}