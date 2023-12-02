namespace Unni.ToDo.Core.Interfaces
{
    public interface IAdminUnitOfWork : IDisposable
    {
        void SaveChanges();
    }
}