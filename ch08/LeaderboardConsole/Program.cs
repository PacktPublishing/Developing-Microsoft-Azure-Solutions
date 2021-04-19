using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static Leaderboard.HttpHelper;

namespace Leaderboard
{
    class Program
    {
        static async Task Main()
        {
            Console.WriteLine("Hit any key to continue.");
            Console.ReadLine();

            try
            {
                var date = new DateTime(2021, 1, 1);

                var b1 = new BoardEntry { UserId = 1, Email = "bob@gmail.com", LastWorkout = date, Name = "Bob Jones", NumOfWorkouts = 61 };
                var b2 = new BoardEntry { UserId = 2, Email = "sarah@gmail.com", LastWorkout = date, Name = "Sarah Smith", NumOfWorkouts = 73 };
                var b3 = new BoardEntry { UserId = 3, Email = "jane@msn.com", LastWorkout = date, Name = "Jane Doe", NumOfWorkouts = 120 };
                var b4 = new BoardEntry { UserId = 4, Email = "rich@mill5.com", LastWorkout = date, Name = "Rich Crane", NumOfWorkouts = 229 };
                var b5 = new BoardEntry { UserId = 5, Email = "dude@yahoo.com", LastWorkout = date, Name = "The Dude", NumOfWorkouts = 323 };
                var b6 = new BoardEntry { UserId = 6, Email = "john@apple.com", LastWorkout = date, Name = "John Pelak", NumOfWorkouts = 208 };
                var b7 = new BoardEntry { UserId = 7, Email = "tim@apple.com", LastWorkout = date, Name = "Tim Cook", NumOfWorkouts = 20 };
                var b8 = new BoardEntry { UserId = 8, Email = "jz@microsoft.com", LastWorkout = date, Name = "Johnathan Z", NumOfWorkouts = 2 };
                var b9 = new BoardEntry { UserId = 9, Email = "claire@msn.com", LastWorkout = date, Name = "Claire Bridges", NumOfWorkouts = 50 };
                var b10 = new BoardEntry { UserId = 10, Email = "ron@outlook.com", LastWorkout = date, Name = "Ron Peter", NumOfWorkouts = 1 };
                var b11 = new BoardEntry { UserId = 11, Email = "joe@facebook.com", LastWorkout = date, Name = "Joe Dean", NumOfWorkouts = 0 };

                await InsertIntoLeaderBoardAsync(b1);
                await InsertIntoLeaderBoardAsync(b2);
                await InsertIntoLeaderBoardAsync(b3);
                await InsertIntoLeaderBoardAsync(b4);
                await InsertIntoLeaderBoardAsync(b5);
                await InsertIntoLeaderBoardAsync(b6);
                await InsertIntoLeaderBoardAsync(b7);
                await InsertIntoLeaderBoardAsync(b8);
                await InsertIntoLeaderBoardAsync(b9);
                await InsertIntoLeaderBoardAsync(b10);
                await InsertIntoLeaderBoardAsync(b11);
            }
            catch
            {
                // FIRST TIME IN WORKS
            }

            var topTen = await GetTop10Async();

            foreach (var b in topTen)
            {
                Console.WriteLine($"{b.User.Name} has {b.NumOfWorkouts} workouts.");
            }

            Console.WriteLine("Hit any key to exit.");
            Console.ReadLine();
        }

        public static async Task InsertIntoLeaderBoardAsync(BoardEntry insertThis)
        {
            const string URL = "http://localhost:7071/api/InsertIntoLeaderboard";

            using (var client = new HttpClient())
            {
                await client.SendAsync(URL, insertThis);
            }
        }

        public static async Task<IEnumerable<Board>> GetTop10Async()
        {
            const string URL = "http://localhost:7071/api/GetTopTen";

            IEnumerable<Board> result;

            using (var client = new HttpClient())
            {
                result = await client.GetAsync<IEnumerable<Board>>(URL);
            }

            return result;
        }
    }
}
