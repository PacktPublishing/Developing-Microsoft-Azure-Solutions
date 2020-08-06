using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DeckOfCards
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CreatedBy
    {
        DeckOfCards = 0,
        User = 1
    }
}
