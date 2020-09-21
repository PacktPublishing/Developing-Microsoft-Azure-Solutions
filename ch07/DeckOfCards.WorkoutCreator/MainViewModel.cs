using AsyncAwaitBestPractices.MVVM;
using DeckOfCards.ImageGallery;
using DeckOfCards.WorkoutCreator;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DeckOfCards.Admin
{
    public class MainViewModel : GalaSoft.MvvmLight.ViewModelBase
    {
        private List<Workout> _workouts = new List<Workout>();
        private Workout _selectedWorkout;

        private List<Exercise> _exercises = new List<Exercise>();

        public MainViewModel()
        {
            Connection = "DefaultEndpointsProtocol=https;EndpointSuffix=core.windows.net;AccountName=docsample;AccountKey=ajZlcpSqWb+c5z8PrVw1spk9rO0W8KHVr8cBp94RkX/a07QNBedZ8uOklwloDj67p0SDeV/zy5C3NtdST2FIeA==";
        }

        public string WorkoutName { get; set; }

        public Workout SelectedWorkout
        {
            get
            {
                return _selectedWorkout;
            }
            set
            {
                _selectedWorkout = value;

                if (_selectedWorkout != null)
                {
                    WorkoutName = _selectedWorkout.Name;
                }

                RaisePropertyChanged(() => WorkoutName);
            }
        }

        public List<Workout> Workouts
        {
            get
            {
                return _workouts;
            }
            set
            {
                _workouts = value;

                RaisePropertyChanged(() => Workouts);
            }
        }

        public List<Exercise> Exercises
        {
            get
            {
                return _exercises;
            }
            set
            {
                _exercises = value;
            }
        }

        public string Connection { get; set; }

        public ICommand RefreshCommand => new RelayCommand(Refresh);

        private void Refresh()
        {
            string connectionString = Connection;

            var workoutsFromTable = TableOperations.GetWorkouts(connectionString);

            var workouts = new List<Workout>();

            foreach (var wo in workoutsFromTable)
            {
                workouts.Add(new Workout
                {
                    WorkoutId = wo.RowKey,
                    Name = wo.Name,
                    Image = wo.Image
                });
            }

            Workouts = workouts;
        }

        public ICommand SaveWorkoutCommand => new RelayCommand(SaveWorkout);

        private void SaveWorkout()
        {
            string connectionString = Connection;

            var workout = new Workout { Name = WorkoutName };

            var entity = new WorkoutEntity
            {
                PartitionKey = workout.WorkoutId,
                RowKey = workout.WorkoutId,
                Name = workout.Name,
                Image = workout.Image
            };

            var e = TableOperations.SaveWorkout(connectionString, entity);

            var workouts = new List<Workout>(_workouts);
            workouts.Add(workout);
            Workouts = workouts;

            SelectedWorkout = workout;
            RaisePropertyChanged(() => SelectedWorkout);
        }

        public ICommand DeleteWorkoutCommand => new RelayCommand(DeleteWorkout);

        private void DeleteWorkout()
        {
            string connectionString = Connection;

            var selected = SelectedWorkout;

            if (selected != null)
            {
                var entity = new WorkoutEntity
                {
                    PartitionKey = selected.WorkoutId,
                    RowKey = selected.WorkoutId
                };

                TableOperations.DeleteWorkout(connectionString, entity);

                RaisePropertyChanged(() => Workouts);
                SelectedWorkout = null;
                RaisePropertyChanged(() => SelectedWorkout);
            }
        }
    }
}
