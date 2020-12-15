using System;
using System.Collections.Generic;

#nullable disable

namespace DeckOfCards
{
    public partial class BoardUser
    {
        public BoardUser()
        {
            Boards = new HashSet<Board>();
        }

        public int UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public DateTime LastWorkout { get; set; }

        public virtual ICollection<Board> Boards { get; set; }
    }
}
