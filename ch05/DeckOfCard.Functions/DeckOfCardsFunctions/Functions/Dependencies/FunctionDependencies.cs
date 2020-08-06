using static Pineapple.Common.Preconditions;

namespace DeckOfCards
{
    internal class FunctionDepedencies : IFunctionDepedencies
    {
        public FunctionDepedencies(IDeckOfCardsStore store)
        {
            CheckIsNotNull(nameof(store), store);

            Store = store;
        }

        public IDeckOfCardsStore Store { get; }
    }
}
