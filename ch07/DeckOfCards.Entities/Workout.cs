using Pineapple.Common;
using System.Collections.Generic;

namespace DeckOfCards
{
    public class Workout
    {
        public Workout()
        {
            WorkoutId = ShortGuid.NewGuid();
        }

        public string WorkoutId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }

        public IEnumerable<Exercise> Exercises { get; set; }
    }
}
