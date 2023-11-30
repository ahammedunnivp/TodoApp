namespace Unni.ToDo.Common.Interfaces
{
    public interface ITodoUnitOfWork : IDisposable
    {
        void SaveChanges();
    }
}
