namespace Klukva.RedKit.Persistence.Migration
{
    public interface IMigrationPipeline<T>
    {
        T Apply(T data);
    }
}