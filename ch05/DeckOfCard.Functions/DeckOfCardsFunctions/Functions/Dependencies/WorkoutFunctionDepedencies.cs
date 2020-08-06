using static Pineapple.Common.Preconditions;

namespace DeckOfCards
{
    internal class WorkoutFunctionDepedencies : IWorkoutFunctionDepedencies
    {
        public WorkoutFunctionDepedencies(IDeckOfCardsStore store, IWorkoutsStore workoutsStore)
        {
            CheckIsNotNull(nameof(store), store);
            CheckIsNotNull(nameof(workoutsStore), workoutsStore);

            Store = store;
            WorkoutsStore = workoutsStore;
        }

        public IDeckOfCardsStore Store { get; }

        public IWorkoutsStore WorkoutsStore { get; }
    }
}
