namespace Unni.Todo.Application.Interfaces
{
    public interface ITodoUnitOfWork : IDisposable
    {
        void SaveChanges();
    }
}
