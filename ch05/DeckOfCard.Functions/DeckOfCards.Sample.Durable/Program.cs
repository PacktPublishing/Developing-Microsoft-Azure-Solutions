using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using DeckOfCards;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

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
                Console.Write("Operations: [W]orkouts, [S]imulate Workout, [Q]uit");
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
                    case ConsoleKey.L:
                        Url = LocalUrl;
                        AuthKey = string.Empty;
                        break;
                    case ConsoleKey.A:
                        Url = AzureUrl;
                        break;
                    case ConsoleKey.Q:
                        continue;
                    case ConsoleKey.W:
                        apiUrl = $@"{Url}/api/GetWorkouts";

                        await HandleOutput(async () =>
                        {
                            var workouts = await GetOutput<List<Workout>>(apiUrl, AuthKey);

                            Console.WriteLine(apiUrl);

                            Console.WriteLine($@"{workouts.Count()} workouts found");
                            Console.WriteLine();
                            foreach (var w in workouts)
                            {
                                Console.WriteLine($@"A workout was performed at {w.Started} with {w.Exercises.Count()} exercises");
                            }
                        });
                        break;
                    case ConsoleKey.S:
                        await HandleOutput(async () =>
                        {
                            apiUrl = $@"{Url}/api/GetExercises";
                            var exercises = (await GetOutput<List<Exercise>>(apiUrl, AuthKey))
                                            .Take(1)
                                            .Select(x => new TrackedExercise { Exercise = x });

                            var workout = new Workout { Exercises = exercises };

                            apiUrl = $@"{Url}/api/StartWorkoutOrchestration";
                            var wr = await PostAndGetOutput<Workout, CheckStatusResponse>(apiUrl, workout, AuthKey);

                            var completeExerciseUrl = $@"{Url}/api/CompleteExercise?id={wr.Id}";

                            bool success = true;

                            Console.WriteLine($@"Workout {wr.Id} started!");

                            try
                            {
                                Console.WriteLine($@"Workout started!");
                                var e = workout.Exercises.First();
                                Console.Write("Hit [C] to complete exercise: ");

                                while (Console.ReadKey().Key != ConsoleKey.C);

                                Console.WriteLine();

                                await Post(completeExerciseUrl, AuthKey);
                                Console.WriteLine($@"Exercise {e.Exercise.Name} completed!");
                            }
                            catch
                            {
                                success = false;
                            }

                            Console.WriteLine($@"Workout {wr.Id} [Success={success}]");
                        });
                        break;
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

        private static async Task Post(string apiUrl, string authHeader = null)
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(apiUrl),
                    Method = HttpMethod.Post,
                };

                request.Headers.Add("x-functions-key", authHeader);

                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
            }
        }

        private static async Task PostInput<T>(string apiUrl, T input, string authHeader = null) where T : class
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsJsonAsync(apiUrl, input, authHeader);
                response.EnsureSuccessStatusCode();
            }
        }

        private static async Task<V> PostAndGetOutput<T,V>(string apiUrl, T input, string authHeader = null) where T : class where V : class
        {
            V output = null;

            using (var client = new HttpClient())
            {
                var response = await client.PostAsJsonAsync(apiUrl, input, authHeader);
                response.EnsureSuccessStatusCode();
                output = await response.Content.ReadAsJsonAsync<V>();
            }

            return output;
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
    }
}
