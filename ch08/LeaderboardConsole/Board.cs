using System;
using System.Collections.Generic;

#nullable disable

namespace Leaderboard
{
    public partial class Board
    {
        public int Id { get; set; }
        public int NumOfWorkouts { get; set; }
        public int UserId { get; set; }

        public virtual BoardUser User { get; set; }
    }
}
