using Microsoft.Azure.Cosmos.Table;
using System;

namespace DeckOfCards.WorkoutCreator
{
    public class ExerciseEntity : TableEntity
    {
        public string Name { get; set; }
    }
}
