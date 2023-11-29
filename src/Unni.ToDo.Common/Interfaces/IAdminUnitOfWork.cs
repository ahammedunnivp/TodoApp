namespace Unni.ToDo.Common.Interfaces
{
    public interface IAdminUnitOfWork : IDisposable
    {
        void SaveChanges();
    }
}