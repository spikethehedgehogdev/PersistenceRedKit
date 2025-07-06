namespace Klukva.RedKit.Persistence.Storage
{
    public interface IStorage
    { 
        bool Exists();
        string Read();
        void Write(string data);
        void Delete();
        void Backup();
    }
}