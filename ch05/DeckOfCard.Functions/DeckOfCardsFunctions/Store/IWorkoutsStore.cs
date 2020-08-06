
using System.Threading.Tasks;

namespace DeckOfCards
{
    public interface IWorkoutsStore
    {
        Task<Workout> StartWorkout(string instanceId, Workout workout);
        Task<Workout> GetWorkout(string instanceId);
        Task<Workout> RemoveWorkout(string instanceId);
    }
}