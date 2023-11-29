namespace Unni.ToDo.API.Data.UnitOfWork
{
    public interface IAdminUnitOfWork : IDisposable
    {
        void SaveChanges();
    }
}