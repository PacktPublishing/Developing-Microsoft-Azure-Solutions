using System;
using System.Collections.Generic;

#nullable disable

namespace DeckOfCards
{
    public partial class Board
    {
        public int Id { get; set; }
        public long NumOfWorkouts { get; set; }
        public int UserId { get; set; }

        public virtual BoardUser User { get; set; }
    }
}
