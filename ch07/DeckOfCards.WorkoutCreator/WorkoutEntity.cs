using Microsoft.Azure.Cosmos.Table;
using System;

namespace DeckOfCards.WorkoutCreator
{
    public class WorkoutEntity : TableEntity
    {
        public WorkoutEntity()
        {
        }

        public string Name { get; set; }

        public string Image { get; set; }
    }
}
