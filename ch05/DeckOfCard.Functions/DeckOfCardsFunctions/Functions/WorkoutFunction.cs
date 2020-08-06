using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using static Pineapple.Common.Preconditions;

namespace DeckOfCards
{
    public partial class WorkoutFunctions
    {
        private readonly IFunctionDepedencies _d;

        public WorkoutFunctions(IFunctionDepedencies d)
        {
            CheckIsNotNull(nameof(d), d);
            _d = d;
        }

        private Task GetTimeoutTask(IDurableOrchestrationContext context, TimeSpan timeout, CancellationTokenSource cts)
        {
            var ct = cts.Token;
            var deadline = context.CurrentUtcDateTime.Add(timeout);
            return context.CreateTimer(deadline, ct);
        }

        [FunctionName("WorkoutOrchestration")]
        public async Task RunOrchestrator([OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var workout = context.GetInput<Workout>();

            var timeout = TimeSpan.FromSeconds(120);

            workout = await context.CallActivityAsync<Workout>("StartWorkout", workout);

            using (var cts = new CancellationTokenSource())
            {
                var timeoutTask = GetTimeoutTask(context, timeout, cts);
                var completeExerciseTask = context.WaitForExternalEvent("CompleteExercise");

                var task = await Task.WhenAny(timeoutTask, completeExerciseTask);

                if (task == completeExerciseTask)
                {
                    cts.Cancel();
                }
            }

            workout = await context.CallActivityAsync<Workout>("CompleteWorkout", workout);
        }

        private TrackedExercise GetNextExercise(Workout workout)
        {
            TrackedExercise next = null;

            foreach (var e in workout.Exercises)
            {
                if (e.Started == null)
                {
                    next = e;
                    break;
                }
            }

            return next;
        }

        private TrackedExercise GetCurrentExercise(Workout workout)
        {
            TrackedExercise next = null;

            foreach (var e in workout.Exercises)
            {
                if (e.Started != null && e.Finished == null)
                {
                    next = e;
                    break;
                }
            }

            return next;
        }

        [FunctionName("CompleteExercise")]
        public async Task<IActionResult> CompleteExercise(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [DurableClient] IDurableOrchestrationClient client,
            ILogger log)
        {
            var instanceId = req.Query["id"];

            var status = await client.GetStatusAsync(instanceId);

            if (status.RuntimeStatus == OrchestrationRuntimeStatus.Running)
            {
                await client.RaiseEventAsync(instanceId, "CompleteExercise");
                return new OkObjectResult("Exercise completed.");
            }

            return await Task.FromResult(new NotFoundResult());
        }

        [FunctionName("StartWorkout")]
        public async Task<Workout> StartWorkout([ActivityTrigger] IDurableActivityContext context,
                                                         ILogger log)
        {
            var instanceId = context.InstanceId;

            var workout = context.GetInput<Workout>();
            var exercise = GetNextExercise(workout);
            exercise.Started = DateTime.Now;

            return await Task.FromResult(workout);
        }


        [FunctionName("CompleteWorkout")]
        public async Task<Workout> CompleteWorkout([ActivityTrigger] IDurableActivityContext context, ILogger log)
        {
            var instanceId = context.InstanceId;

            var workout = context.GetInput<Workout>();
            var exercise = GetCurrentExercise(workout);
            exercise.Finished = DateTime.Now;

            await _d.Store.AddWorkout(workout);

            return workout;
        }

        [FunctionName("StartWorkoutOrchestration")]
        public async Task<IActionResult> HttpStart(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            var workout = await req.Deserialize<Workout>();

            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("WorkoutOrchestration", workout);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}