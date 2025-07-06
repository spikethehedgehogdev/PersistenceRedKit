
namespace Klukva.RedKit.Persistence.Serialization
{
    public interface ISerializationStrategy<T>
    {
        string Serialize(T data);
        T Deserialize(string raw);
    }
}