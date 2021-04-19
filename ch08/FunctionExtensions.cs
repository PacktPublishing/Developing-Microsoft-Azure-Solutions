using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using static Pineapple.Common.Preconditions;

namespace Leaderboard
{
    internal static class FunctionExtensions
    {
        public static async Task<T> Deserialize<T>(this HttpRequest req)
        {
            CheckIsNotNull(nameof(req), req);

            var sr = new StreamReader(req.Body);
            string requestBody = await sr.ReadToEndAsync();

            return JsonConvert.DeserializeObject<T>(requestBody);
        }

        public static async Task Serialize<T>(T serializeThis, Stream s)
        {
            using (StreamWriter writer = new StreamWriter(s))
            {
                using (JsonTextWriter jsonWriter = new JsonTextWriter(writer))
                {
                    var ser = new JsonSerializer();
                    ser.Serialize(jsonWriter, serializeThis);
                    jsonWriter.Flush();
                }
            }

            await Task.CompletedTask;
        }
    }
}
