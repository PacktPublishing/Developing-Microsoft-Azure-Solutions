using System;
using System.Collections.Generic;
using Pineapple.Common;
using System.Linq;
using Newtonsoft.Json;

namespace DeckOfCards
{
    public class Workout
    {
        public Workout()
        {
            WorkoutId = ShortGuid.NewGuid();
        }

        public string WorkoutId { get; set; }
        

        [JsonIgnore]
        public DateTime? Started
        {
            get
            {
                return Exercises.Where(x => x.Started != null).Min(x => x.Started);
            }
        }

        [JsonIgnore]
        public DateTime? Finished
        {
            get
            {
                return Exercises.Where(x => x.Finished != null).Max(x => x.Finished);
            }
        }

        public IEnumerable<TrackedExercise> Exercises { get; set; }
    }

    public class TrackedExercise
    {
        public Exercise Exercise { get; set; }

        public DateTime? Started { get; set; }

        public DateTime? Finished { get; set; }
    }

    public enum ExerciseState
    {
        NotStarted = 0,
        Started  = 1,
        Completed = 2
    }
}
