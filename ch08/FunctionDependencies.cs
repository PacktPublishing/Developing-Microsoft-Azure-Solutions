using static Pineapple.Common.Preconditions;

namespace Leaderboard
{

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
