using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using static Pineapple.Common.Preconditions;

namespace DeckOfCards.InMemory
{
    public class InMemoryStore : IDeckOfCardsStore
    {
        #region Static Variables
        private static volatile IImmutableDictionary<string, Deck> _decks = ImmutableDictionary<string, Deck>.Empty;
        private static volatile IImmutableDictionary<string, Exercise> _exercises = ImmutableDictionary<string, Exercise>.Empty;
        private static volatile IImmutableDictionary<string, Workout> _workouts = ImmutableDictionary<string, Workout>.Empty;
        private static readonly IDeckOfCardsStore _store;
        #endregion

        static InMemoryStore()
        {
            _store = new InMemoryStore();
        }

        public static IDeckOfCardsStore GetStore()
        {
            return _store;
        }

        internal InMemoryStore()
        {
        }

        public async Task<IEnumerable<Deck>> GetDecks()
        {
            return await Task.FromResult(_decks.Values.OrderBy(x => x.Cards.Count()));
        }

        public async Task<IEnumerable<Exercise>> GetExercises()
        {
            return await Task.FromResult(_exercises.Values.OrderBy(x => x.Name));
        }

        public async Task<IEnumerable<Workout>> GetWorkouts()
        {
            return await Task.FromResult(_workouts.Values.OrderBy(x => x.Started));
        }

        public async Task AddDeck(Deck deck)
        {
            CheckIsNotNull(nameof(deck), deck);

            _decks = _decks.Add(deck.DeckId, deck);

            await Task.CompletedTask;
        }

        public async Task AddExercise(Exercise exercise)
        {
            CheckIsNotNull(nameof(exercise), exercise);

            _exercises = _exercises.Add(exercise.ExerciseId, exercise);

            await Task.CompletedTask;
        }

        public async Task AddWorkout(Workout workout)
        {
            CheckIsNotNull(nameof(workout), workout);

            _workouts = _workouts.Add(workout.WorkoutId, workout);

            await Task.CompletedTask;
        }
    }
}
