using System;

namespace Leaderboard
{
    public class BoardEntry
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public int NumOfWorkouts { get; set; }
        public DateTime LastWorkout { get; set; }

    }
}