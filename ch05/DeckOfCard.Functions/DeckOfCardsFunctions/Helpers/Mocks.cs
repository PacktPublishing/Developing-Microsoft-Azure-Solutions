using System.Collections.Generic;
using Pineapple.Common;
using static Pineapple.Common.Preconditions;

namespace DeckOfCards
{
    public static class Mocks
    {
        public static void LoadMocks(IDeckOfCardsWriteOnlyStore store)
        {
            CheckIsNotNull(nameof(store), store);

            foreach (var d in GetDecks())
            {
                store.AddDeck(d);
            }

            foreach (var e in GetExercises())
            {
                store.AddExercise(e);
            }
        }

        public static IEnumerable<Deck> GetDecks()
        {
            var decks = new List<Deck>();

            var standardSuits = new[] { Suit.Clubs, Suit.Hearts, Suit.Spades, Suit.Diamonds };

            decks.Add(Deck.Standard);

            var d = new Deck("Quick")
                    .Add(FaceOrNumber.Jack, standardSuits)
                    .Add(FaceOrNumber.Queen, standardSuits)
                    .Add(FaceOrNumber.King, standardSuits)
                    .Add(FaceOrNumber.Ace, standardSuits);
            decks.Add(d);

            return decks;
        }

        public static IEnumerable<Exercise> GetExercises()
        {
            var exercises = new List<Exercise>();

            exercises.Add(new Exercise { ExerciseId = ShortGuid.NewGuid(), Name = "Burpees", Description = "", CreatedBy = CreatedBy.DeckOfCards });
            exercises.Add(new Exercise { ExerciseId = ShortGuid.NewGuid(), Name = "Pushups", Description = "", CreatedBy = CreatedBy.DeckOfCards });
            exercises.Add(new Exercise { ExerciseId = ShortGuid.NewGuid(), Name = "Sit-ups", Description = "", CreatedBy = CreatedBy.DeckOfCards });
            exercises.Add(new Exercise { ExerciseId = ShortGuid.NewGuid(), Name = "Lunges", Description = "", CreatedBy = CreatedBy.DeckOfCards });
            exercises.Add(new Exercise { ExerciseId = ShortGuid.NewGuid(), Name = "Planks", Description = "", CreatedBy = CreatedBy.DeckOfCards });

            return exercises;
        }

    }
}
