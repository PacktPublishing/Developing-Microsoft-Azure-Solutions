namespace DeckOfCards
{
    public interface IWorkoutFunctionDepedencies : IFunctionDepedencies
    {
        IWorkoutsStore WorkoutsStore { get; }
    }
}