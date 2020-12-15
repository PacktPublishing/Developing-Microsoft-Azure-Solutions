using static Pineapple.Common.Preconditions;

namespace DeckOfCards
{
    public interface IFunctionDependencies
    {
        LeaderboardContext Store { get; }
    }

    internal class FunctionDepedencies : IFunctionDependencies
    {
        public FunctionDepedencies(LeaderboardContext store)
        {
            CheckIsNotNull(nameof(store), store);

            Store = store;
        }

        public LeaderboardContext Store { get; }
    }
}
