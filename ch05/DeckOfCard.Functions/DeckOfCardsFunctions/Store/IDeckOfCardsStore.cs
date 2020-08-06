using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeckOfCards
{
    public interface IDeckOfCardsStore : IDeckOfCardsReadOnlyStore, IDeckOfCardsWriteOnlyStore
    {
    }

    public interface IDeckOfCardsReadOnlyStore
    {
        Task<IEnumerable<Deck>> GetDecks();
        Task<IEnumerable<Exercise>> GetExercises();
        Task<IEnumerable<Workout>> GetWorkouts();
    }

    public interface IDeckOfCardsWriteOnlyStore
    {
        Task AddDeck(Deck deck);
        Task AddExercise(Exercise exercise);
        Task AddWorkout(Workout workout);
    }
}
