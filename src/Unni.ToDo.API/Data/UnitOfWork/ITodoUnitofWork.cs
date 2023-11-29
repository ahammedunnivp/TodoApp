namespace Unni.ToDo.API.Data.UnitOfWork
{
    public interface ITodoUnitOfWork : IDisposable
    {
        void SaveChanges();
    }
}
