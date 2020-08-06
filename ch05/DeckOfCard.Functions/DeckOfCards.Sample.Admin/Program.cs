using DeckOfCards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DeckOfCardsSample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string LocalUrl = "http://localhost:7071";
            string AzureUrl = "https://deckofcards1000.azurewebsites.net";
            string Url = LocalUrl;
            string AuthKey = string.Empty;

            Console.Clear();

            var key = ConsoleKey.Enter;

            while (key != ConsoleKey.Q)
            {
                Console.WriteLine("[A]zure, [L]ocalhost");
                Console.WriteLine("[M]aster Key, [H]ost Key, [F]unction Key(GetDecks), [N]o Key");
                Console.WriteLine();
                Console.WriteLine($@"Url = {Url}");
                Console.WriteLine($@"Key = {AuthKey}");
                Console.Write("Operations: [D]ecks, [1] Add French Deck, [Q]uit");
                Console.Write(" -> ");
                string apiUrl;
                key = Console.ReadKey().Key;
                Console.WriteLine();
                Console.WriteLine();

                switch (key)
                {
                    case ConsoleKey.F:
                        AuthKey = $@"y3ujRk7t0tECsiabiKinRnfBTnXogjRtcahM2ga5ZMKlWbBd7/iYwA==";
                        break;
                    case ConsoleKey.M:
                        AuthKey = $@"2wua1lRau/ASzxNVzEGVbioLaAxT9sdXpkqi6MAnrr3g7MW/7ICYpg==";
                        break;
                    case ConsoleKey.H:
                        AuthKey = $@"EP7jrMZvdUghuFHi8Ujr2pMGBpThfekHcTeTRgO7T8va5DbKpfy/ag==";
                        break;
                    case ConsoleKey.N:
                        AuthKey = string.Empty;
                        break;
                    case ConsoleKey.D:
                        apiUrl = $@"{Url}/api/GetDecks";

                        await HandleOutput(async () =>
                        {
                            var decks = await GetOutput<List<Deck>>(apiUrl, AuthKey);

                            Console.WriteLine(apiUrl);

                            Console.WriteLine($@"{decks.Count()} decks found");
                            Console.WriteLine();
                            foreach (var d in decks)
                            {
                                Console.WriteLine($@"{d.Name} deck has {d.Cards.Count()} cards");
                            }
                        });

                        break;
                    case ConsoleKey.D1:
                        apiUrl = $@"{Url}/api/AddDecks";

                        await HandleOutput(async () =>
                        {
                            var decks = new [] { GetFrenchDeck() };

                            await PostInput(apiUrl, decks, AuthKey);

                            Console.WriteLine(apiUrl);
                            Console.WriteLine("Success!");
                        });

                        break;
                    case ConsoleKey.L:
                        Url = LocalUrl;
                        AuthKey = string.Empty;
                        break;
                    case ConsoleKey.A:
                        Url = AzureUrl;
                        break;
                    case ConsoleKey.Q:
                        continue;
                }

                Console.Clear();
            }
        }

        private static async Task HandleOutput(Func<Task> action)
        {
            try
            {
                await action();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine();
            Console.WriteLine("Press [Enter] to continue.");
            Console.ReadLine();
        }

        private static async Task PostInput<T>(string apiUrl, T input, string authHeader = null) where T : class
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsJsonAsync(apiUrl, input, authHeader);
                response.EnsureSuccessStatusCode();
            }
        }

        private static async Task<T> GetOutput<T>(string apiUrl, string authHeader = null) where T : class
        {
            T output = null;

            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(apiUrl),
                    Method = HttpMethod.Get,
                };

                request.Headers.Add("x-functions-key", authHeader);

                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                output = await response.Content.ReadAsJsonAsync<T>();
            }

            return output;
        }

        private static Deck GetFrenchDeck()
        {
            var standardSuits = new[] { Suit.Clubs, Suit.Hearts, Suit.Spades, Suit.Diamonds };
            var jokerSuits = new[] { Suit.Black, Suit.Red };

            var d = new Deck("French")
                    .Add(FaceOrNumber.Ace, standardSuits)
                    .Add(FaceOrNumber.Two, standardSuits)
                    .Add(FaceOrNumber.Three, standardSuits)
                    .Add(FaceOrNumber.Four, standardSuits)
                    .Add(FaceOrNumber.Five, standardSuits)
                    .Add(FaceOrNumber.Six, standardSuits)
                    .Add(FaceOrNumber.Seven, standardSuits)
                    .Add(FaceOrNumber.Eight, standardSuits)
                    .Add(FaceOrNumber.Nine, standardSuits)
                    .Add(FaceOrNumber.Ten, standardSuits)
                    .Add(FaceOrNumber.Jack, standardSuits)
                    .Add(FaceOrNumber.Queen, standardSuits)
                    .Add(FaceOrNumber.King, standardSuits)
                    .Add(FaceOrNumber.Joker, jokerSuits);

            return d;
        }
    }
}
