using AsyncAwaitBestPractices.MVVM;
using GalaSoft.MvvmLight.Command;
using Microsoft.Extensions.Configuration;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using static Pineapple.Common.Preconditions;

namespace DeckOfCards.ImageGallery
{
    public class MainViewModel : GalaSoft.MvvmLight.ViewModelBase
    {
        private const string STORAGE_ACCOUNT_CONNECTION = "StorageAccountConnection";
        private const string IMAGE_CONTAINER = "images";
        private bool _connected;
        private ImageItem _selectedItem;

        public MainViewModel(IConfiguration configuration)
        {
            CheckIsNotNull(nameof(configuration), configuration);

            Connection = configuration.GetConnectionString(STORAGE_ACCOUNT_CONNECTION) ?? string.Empty;
            SelectedItem = null;
            Images = new List<ImageItem>();
        }
        
        public string Connection { get; set; }

        public string CurrentDirectory { get; private set; }
        public ICommand ConnectCommand => new AsyncCommand(Connect);
        public ICommand AddCommand => new AsyncCommand(Add, (o) => _connected);
        public ICommand DeleteCommand => new AsyncCommand(Delete, (o) => _connected && SelectedItem != null);

        public List<ImageItem> Images { get; private set; }

        public ImageItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                RaisePropertyChanged(() => DeleteCommand);
            }
        }

        private async Task Add()
        {
            var fd = new OpenFileDialog();

            fd.Filter = "Image files (*.jpg)|*.jpg";

            var result = fd.ShowDialog();

            if (result.HasValue && result.Value)
            {
                var source = fd.FileName;
                var sourceFileInfo = new FileInfo(source);

                var destination = Path.Combine(CurrentDirectory, sourceFileInfo.Name);

                File.Copy(source, destination);
                await BlobOperations.UploadBlobAsync(Connection, IMAGE_CONTAINER, destination);

                var images = new List<ImageItem>(Images);
                images.Add(new ImageItem(destination));
                Images = images;
                RaisePropertyChanged(() => Images);
                
                Process.Start("explorer.exe", CurrentDirectory);
            }
        }

        private async Task Delete()
        {
            var selected = SelectedItem;

            if (selected != null)
            {
                File.Delete(selected.FullName);
                await BlobOperations.DeleteBlobAsync(Connection, IMAGE_CONTAINER, selected.FileName);

                var images = new List<ImageItem>(Images);
                images.Remove(selected);
                Images = images;
                RaisePropertyChanged(() => Images);

                Process.Start("explorer.exe", CurrentDirectory);
            }
        }

        private async Task Connect()
        {
            var connection = Connection;

            try
            {
                CheckDoesContainerExist(connection);
            }
            catch (Exception ex)
            {
                await Dispatcher.CurrentDispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show(ex.Message);
                });
            }

            CurrentDirectory = await BlobOperations.GetBlobsAsync(connection, IMAGE_CONTAINER);
            
            Images = GetImagesFromDirectory(CurrentDirectory);
            RaisePropertyChanged(() => Images);

            Process.Start("explorer.exe", CurrentDirectory);
            _connected = true;
            RaisePropertyChanged(() => AddCommand);
            RaisePropertyChanged(() => DeleteCommand);
        }

        private static void CheckDoesContainerExist(string connection)
        {
            if (!ContainerOperations.Exists(connection, IMAGE_CONTAINER))
            {
                ContainerOperations.CreateContainer(connection, IMAGE_CONTAINER);
            }
        }

        private static List<ImageItem> GetImagesFromDirectory(string directory)
        {
            var files = Directory.GetFiles(directory, "*.jpg");

            var images = new List<ImageItem>();

            foreach (var file in files)
            {
                images.Add(new ImageItem(file));
            }

            return images;
        }
    }
}
