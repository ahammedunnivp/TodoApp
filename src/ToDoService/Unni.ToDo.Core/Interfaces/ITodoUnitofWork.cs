namespace Unni.ToDo.Core.Interfaces
{
    public interface ITodoUnitOfWork : IDisposable
    {
        void SaveChanges();
    }
}
