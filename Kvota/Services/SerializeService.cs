using System.Text.Encodings.Web;
using System.Text.Json;
using Kvota.Constants;
using Kvota.Interfaces;
using Kvota.Models.Content;
using Microsoft.Build.Evaluation;

namespace Kvota.Services
{
    public class SerializeService<T>:ISerializeService<T>
    {
        public async Task<T> Serialize(string path, T obj)
        {
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            await using var createStream = File.Create(path);
            await JsonSerializer.SerializeAsync(createStream, obj, options);
            await createStream.DisposeAsync();
            return obj;
        }

        public async Task<T> DeSerialize(string path)
        {
            await using var openStream = File.OpenRead(path);
            return (await JsonSerializer.DeserializeAsync<T>(openStream))!;
        }

        public async Task<IEnumerable<T>> SerializeCollection(string path, IEnumerable<T> collection)
        {
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            await using var createStream = File.Create(path);
            await JsonSerializer.SerializeAsync(createStream, collection, options);
            await createStream.DisposeAsync();
            return collection;
        }

        public async Task<IEnumerable<T>> DeSerializeCollection(string path)
        {
            await using var openStream = File.OpenRead(path);
            return (await JsonSerializer.DeserializeAsync<IEnumerable<T>>(openStream))!;
        }
    }
}
