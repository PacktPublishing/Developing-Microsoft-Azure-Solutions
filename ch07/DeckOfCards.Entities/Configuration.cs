using System;
using System.Collections.Generic;

namespace DeckOfCards
{
    public class Configuration
    {
        private List<Workout> _workouts;

        public Configuration()
        {
            _workouts = new List<Workout>();
        }


        public static Configuration Empty => new Configuration { Version = new Version("0.0.0.0") };

        public Version Version { get; set; }

        public IEnumerable<Workout> Workouts
        {
            get { return _workouts; }
            set
            {
                _workouts = new List<Workout>(value);
            }
        }

        public Workout AddWorkout(Workout workout)
        {
            _workouts.Add(workout);
            return workout;
        }

        public Workout DeleteWorkout(Workout workout)
        {
            _workouts.Remove(workout);
            return workout;
        }
    }
}
