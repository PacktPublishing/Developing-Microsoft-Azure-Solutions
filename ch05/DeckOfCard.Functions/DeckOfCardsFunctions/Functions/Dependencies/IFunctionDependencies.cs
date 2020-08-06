namespace DeckOfCards
{
    public interface IFunctionDepedencies
    {
        IDeckOfCardsStore Store { get; }
    }
}