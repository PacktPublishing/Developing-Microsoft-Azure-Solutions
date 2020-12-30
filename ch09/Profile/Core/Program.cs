using System;
using System.Threading.Tasks;
using static Pineapple.Common.Preconditions;

namespace DeckOfCards
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Creating profile");

            var id = Guid.NewGuid();

            var profileToSave = new Profile
            {
                UserId = id,
                FirstName = "John",
                LastName = "Doe",
                Favorite = new Workout { Name = "Ab Blast", WorkoutId = Guid.NewGuid() },
                Location = new Location { State = "MA", City = "Boston" },
            };

            var context = new ProfileContext();

            Console.WriteLine("Adding profile");
            await context.AddProfile(profileToSave);

            Console.WriteLine("Reading profile");
            var profile = await context.GetProfile(id);
            CheckIsNotNull(nameof(profile), profile);

            Console.WriteLine("Complete! Hit any key to exit.");
            Console.ReadLine();
        }
    }
}
