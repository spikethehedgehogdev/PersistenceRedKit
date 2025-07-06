using System;
using System.IO;
using System.Text;

namespace Klukva.RedKit.Persistence.Serialization
{
    public abstract class BinarySerializationStrategy<T> : ISerializationStrategy<T>
    {
        public string Serialize(T data)
        {
            try
            {
                using var memoryStream = new MemoryStream();
                using var writer = new BinaryWriter(memoryStream, Encoding.UTF8);

                WriteData(writer, data);

                writer.Flush();
                return Convert.ToBase64String(memoryStream.ToArray());
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Serialization failed: {ex.Message}", ex);
            }
        }

        public T Deserialize(string raw)
        {
            try
            {
                var bytes = Convert.FromBase64String(raw);
                using var memoryStream = new MemoryStream(bytes);
                using var reader = new BinaryReader(memoryStream, Encoding.UTF8);

                return ReadData(reader);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Deserialization failed: {ex.Message}", ex);
            }
        }

        protected abstract void WriteData(BinaryWriter writer, T data);
        protected abstract T ReadData(BinaryReader reader);
    }

}