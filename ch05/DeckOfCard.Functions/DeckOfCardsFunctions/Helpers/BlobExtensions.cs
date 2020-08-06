using Azure.Storage.Blobs;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace DeckOfCards
{
    public static class BlobExtensions
    {
        public static async Task Save<T>(this BlobContainerClient containerClient, string instanceId, T instance, bool overwrite = false) where T: class
        {
            var blobClient = containerClient.GetBlobClient(instanceId);

            if (!overwrite && await blobClient.ExistsAsync())
            {
                return;
            }

            using (var stream = new MemoryStream())
            {
                SaveToStream(stream, instance);
                var response = await blobClient.UploadAsync(stream);
            }
        }

        public static async Task Remove(this BlobContainerClient containerClient, string instanceId)
        {
            var blobClient = containerClient.GetBlobClient(instanceId);
            await blobClient.DeleteIfExistsAsync();
        }

        public static async Task<T> Load<T>(this BlobContainerClient containerClient, string instanceId) where T: class
        {
            var blobClient = containerClient.GetBlobClient(instanceId);

            T result = null;

            if (await blobClient.ExistsAsync())
            {
                using (var stream = new MemoryStream())
                {
                    var response = await blobClient.DownloadToAsync(stream);
                    stream.Position = 0;
                    result = await LoadFromStream<T>(stream);
                }
            }

            return result;
        }

        private static async Task<T> LoadFromStream<T>(Stream s) where T: class
        {
            T result = null;

            using (var sr = new StreamReader(s))
            {
                string json = await sr.ReadToEndAsync();
                result = JsonConvert.DeserializeObject<T>(json);
            }

            return result;
        }

        private static void SaveToStream<T>(Stream s, T instance) where T: class
        {
            var json = JsonConvert.SerializeObject(instance);

            StreamWriter writer = new StreamWriter(s);
            writer.Write(json);
            writer.Flush();
            s.Position = 0;
        }
    }
}
