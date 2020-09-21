using DeckOfCards.WorkoutCreator;
using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DeckOfCards.ImageGallery
{
    public static class TableOperations
    {
const string WORKOUTS_TABLE_NAME = "Workouts";

private static CloudTable GetWorkoutsTable(string connectionString)
{
    var storageAccount = CloudStorageAccount.Parse(connectionString);
    var tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
    return tableClient.GetTableReference(WORKOUTS_TABLE_NAME);
}

public static List<WorkoutEntity> GetWorkouts(string connectionString)
{
    var table = GetWorkoutsTable(connectionString);
    var query = new TableQuery<WorkoutEntity>();
    return table.ExecuteQuery(query).ToList();
}

        public static WorkoutEntity GetWorkout(string connectionString, string workoutId)
        {
            var table = GetWorkoutsTable(connectionString);
            var operation = TableOperation.Retrieve<WorkoutEntity>(workoutId, workoutId);
            var result = table.Execute(operation);
            return result.Result as WorkoutEntity;
        }

public static WorkoutEntity SaveWorkout(string connectionString, WorkoutEntity workout)
{
    var table = GetWorkoutsTable(connectionString);
    var operation = TableOperation.InsertOrReplace(workout);
    var result = table.Execute(operation);
    return result.Result as WorkoutEntity;
}

public static WorkoutEntity DeleteWorkout(string connectionString, WorkoutEntity workout)
{
    workout.ETag = "*";
    var table = GetWorkoutsTable(connectionString);
    var operation = TableOperation.Delete(workout);
    var result = table.Execute(operation);
    return workout;
}

        public static List<ExerciseEntity> GetExercisesByWorkout(string workoutId)
        {
            return null;
        }
    }
}
