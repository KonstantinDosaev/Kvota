namespace Kvota.Interfaces
{
    public interface ISerializeService<T>
    {
        Task<T> Serialize(string path, T obj);
        Task<T> DeSerialize(string path);
        Task<IEnumerable<T>> SerializeCollection(string path, IEnumerable<T> collection);
        Task<IEnumerable<T>> DeSerializeCollection(string path);
    }
}
