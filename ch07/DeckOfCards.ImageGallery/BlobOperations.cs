using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Azure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Documents;
using static Pineapple.Common.Cleanup;

namespace DeckOfCards.ImageGallery
{
    public static class BlobOperations
    {
        public static async Task UploadBlobAsync(string connectionString,
                                            string containerName,
                                            string fileName)
        {
            var file = new FileInfo(fileName);
            var blob = new BlobClient(connectionString, containerName, file.Name);

            await blob.DeleteIfExistsAsync();
            await blob.UploadAsync(fileName);
        }

        public static async Task DeleteBlobAsync(string connectionString,
                                                    string containerName,
                                                    string name)
        {
            var blob = new BlobClient(connectionString, containerName, name);
            await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
        }

        public static List<string> GetBlobNames(string connectionString, string containerName)
        {
            var serviceClient = new BlobServiceClient(connectionString);
            var containerClient = serviceClient.GetBlobContainerClient(containerName);

            var blobs = containerClient.GetBlobs();

            var names = new List<string>();

            foreach (var b in blobs)
            {
                names.Add(b.Name);
            }

            return names;
        }

        public static async Task<string> GetBlobsAsync(string connectionString, string containerName)
        {
            var serviceClient = new BlobServiceClient(connectionString);
            var containerClient = serviceClient.GetBlobContainerClient(containerName);

            var blobs = containerClient.GetBlobsAsync();

            var tempPath = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(Path.GetRandomFileName()));

            Directory.CreateDirectory(tempPath);

            await foreach (var b in blobs)
            {
                var downloadInfo = await containerClient.GetBlobClient(b.Name).DownloadAsync();
                var pathToBlob = Path.Combine(tempPath, b.Name);

                var downloadBuffer = new byte[81920];
                int bytesRead;
                int totalBytesDownloaded = 0;

                var blobToDownload = downloadInfo.Value;
                var outputFile = File.OpenWrite(pathToBlob);

                try
                {
                    while ((bytesRead = blobToDownload.Content
                        .Read(downloadBuffer, 0, downloadBuffer.Length)) != 0)
                    {
                        outputFile.Write(downloadBuffer, 0, bytesRead);
                        totalBytesDownloaded += bytesRead;
                    }
                }
                finally
                {
                    SafeMethod(blobToDownload.Content.Close);
                    SafeMethod(blobToDownload.Content.Dispose);
                    SafeMethod(blobToDownload.Dispose);
                    SafeMethod(outputFile.Close);
                    SafeMethod(outputFile.Dispose);
                }
            }

            return tempPath;
        }
    }
}
