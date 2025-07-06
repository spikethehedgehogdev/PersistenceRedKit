using Klukva.RedKit.Persistence.Common;

namespace Klukva.RedKit.Persistence.Repository
{
    public interface IPersistentRepository<T>
    {
        void Save(T data);
        Result<T> TryLoad();
        void Delete();
    }
}