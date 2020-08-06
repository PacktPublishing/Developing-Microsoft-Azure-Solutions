using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using static Pineapple.Common.Preconditions;

namespace DeckOfCards
{
    public class DataFunctions
    {
        private IFunctionDepedencies _d;

        public DataFunctions(IFunctionDepedencies d)
        {
            CheckIsNotNull(nameof(d), d);
            _d = d;
        }

        [FunctionName("GetDecks")]
        public async Task<IEnumerable<Deck>> GetDecks(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            IEnumerable<Deck> result;

            try
            {
                result = await _d.Store.GetDecks();
            }
            catch (Exception ex)
            {
                log.LogError(ex, nameof(GetDecks));
                throw;
            }

            return await Task.FromResult(result);
        }

        [FunctionName("GetExercises")]
        public async Task<IEnumerable<Exercise>> GetExercises(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            IEnumerable<Exercise> result;

            try
            {
                result = await _d.Store.GetExercises();
            }
            catch (Exception ex)
            {
                log.LogError(ex, nameof(GetExercises));
                throw;
            }

            return await Task.FromResult(result);
        }

        [FunctionName("GetWorkouts")]
        public async Task<IEnumerable<Workout>> GetWorkouts(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            IEnumerable<Workout> result;

            try
            {
                result = await _d.Store.GetWorkouts();
            }
            catch (Exception ex)
            {
                log.LogError(ex, nameof(GetWorkouts));
                throw;
            }

            return await Task.FromResult(result);
        }
    }
}
