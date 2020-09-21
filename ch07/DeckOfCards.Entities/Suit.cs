using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DeckOfCards
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Suit
    {
        None = 0,
        Hearts = 1,
        Diamonds = 2,
        Spades = 3,
        Clubs = 4,

        Black = 1001,
        Red = 1002,
    }
}
