using System;

namespace DeckOfCards
{
    public class Exercise
    {
        public Exercise()
        {
        }

        public string ExerciseId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public CreatedBy CreatedBy { get; set; }

        public bool Enabled { get; set; }
    }
}
