using Azure.Storage.Blobs;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Threading.Tasks;
using static Pineapple.Common.Cleanup;
using static Pineapple.Common.Preconditions;

namespace DeckOfCards
{
    public class WorkoutsStore : IWorkoutsStore
    {
        private const string DeckOfCardsConnectionString = "deckofcardsstorage";
        private const string DeckOfCardsWorkoutsContainer = "deckofcardworkouts";

        private readonly string _storageConnection;

        public WorkoutsStore()
        {
            _storageConnection = Environment.GetEnvironmentVariable(DeckOfCardsConnectionString);
            var blobServiceClient = new BlobServiceClient(_storageConnection);
            
            SafeMethod(() => blobServiceClient.CreateBlobContainer(DeckOfCardsWorkoutsContainer));
        }

        private BlobContainerClient GetContainerClient()
        {
            return new BlobContainerClient(_storageConnection, DeckOfCardsWorkoutsContainer);
        }

        public async Task<Workout> StartWorkout(string instanceId, Workout workout)
        {
            CheckIsNotNullOrWhitespace(nameof(instanceId), instanceId);
            CheckIsNotNull(nameof(workout), workout);

            var blobClient = GetContainerClient();
            await blobClient.Save(instanceId, workout, false);

            return workout;
        }

        public async Task<Workout> UpdateWorkout(string instanceId, Workout workout)
        {
            CheckIsNotNullOrWhitespace(nameof(instanceId), instanceId);

            var blobClient = GetContainerClient();
            return await blobClient.Save(instanceId, workout, true);
        }

        public async Task<Workout> GetWorkout(string instanceId)
        {
            CheckIsNotNullOrWhitespace(nameof(instanceId), instanceId);

            var blobClient = GetContainerClient();
            return await blobClient.Load<Workout>(instanceId);
        }

        public async Task<Workout> RemoveWorkout(string instanceId)
        {
            CheckIsNotNullOrWhitespace(nameof(instanceId), instanceId);

            var blobClient = GetContainerClient();
            var workout = await blobClient.Load<Workout>(instanceId);
            await blobClient.Remove(instanceId);

            return workout;
        }
    }
}
