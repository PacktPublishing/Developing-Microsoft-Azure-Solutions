using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using static Pineapple.Common.Cleanup;
using static Pineapple.Common.Preconditions;

namespace Shrinkify
{
    public static class BlobOperations
    {
        public static async Task<Uri> UploadBlobAsync(string connectionString,
                                            string containerName,
                                            string fileName,
                                            string contentType = null,
                                            bool forceDelete = false)
        {
            CheckIsNotNullOrWhitespace(nameof(connectionString), connectionString);
            CheckIsNotNullOrWhitespace(nameof(containerName), containerName);
            CheckIsNotNullOrWhitespace(nameof(fileName), fileName);
            
            var file = new FileInfo(fileName);
            CheckIsNotCondition(nameof(fileName), !file.Exists, () => $"File does not exist. [{fileName}].");

            var blob = new BlobClient(connectionString, containerName, file.Name);

            if (forceDelete)
            {
                await blob.DeleteIfExistsAsync();
            }

            if (string.IsNullOrWhiteSpace(contentType))
            {
                await blob.UploadAsync(fileName);
            }
            else
            {
                var o = new BlobUploadOptions
                {
                    HttpHeaders = new BlobHttpHeaders
                    {
                        ContentType = contentType
                    }
                };

                await blob.UploadAsync(fileName, o);
            }

            return blob.Uri;
        }

        public static async Task<Uri> UploadBlobAsync(string connectionString,
                                            string containerName,
                                            string fileName,
                                            Stream fileStream,
                                            string contentType = null,
                                            bool forceDelete = false)
        {
            CheckIsNotNullOrWhitespace(nameof(connectionString), connectionString);
            CheckIsNotNullOrWhitespace(nameof(containerName), containerName);
            CheckIsNotNullOrWhitespace(nameof(fileName), fileName);
            CheckIsNotNull(nameof(fileStream), fileStream);

            var blob = new BlobClient(connectionString, containerName, fileName);

            if (forceDelete)
            {
                await blob.DeleteIfExistsAsync();
            }

            if (string.IsNullOrWhiteSpace(contentType))
            {
                await blob.UploadAsync(fileStream);
            }
            else
            {
                var o = new BlobUploadOptions
                {
                    HttpHeaders = new BlobHttpHeaders
                    {
                        ContentType = contentType
                    }
                };

                await blob.UploadAsync(fileStream, o);
            }

            return ReturnUri(blob.Uri);
        }

        private static Uri ReturnUri(Uri url)
        {
            return new Uri(url.ToString().Replace("%2F", "/"));
        }

        public static async Task DeleteBlobAsync(string connectionString,
                                                    string containerName,
                                                    string fileName)
        {
            CheckIsNotNullOrWhitespace(nameof(connectionString), connectionString);
            CheckIsNotNullOrWhitespace(nameof(containerName), containerName);
            CheckIsNotNullOrWhitespace(nameof(fileName), fileName);

            var blob = new BlobClient(connectionString, containerName, fileName);
            await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
        }

        public static async Task<bool> FileOrFolderExists(string connectionString, string containerName, string fileOrFolder)
        {
            CheckIsNotNullOrWhitespace(nameof(connectionString), connectionString);
            CheckIsNotNullOrWhitespace(nameof(containerName), containerName);
            CheckIsNotNullOrWhitespace(nameof(fileOrFolder), fileOrFolder);

            var serviceClient = new BlobServiceClient(connectionString);
            var containerClient = serviceClient.GetBlobContainerClient(containerName);

            var pages = containerClient
                .GetBlobsByHierarchyAsync(prefix: fileOrFolder, delimiter: "/")
                .AsPages();

            await foreach (var p in pages)
            {
                if (p.Values.Count > 0)
                    return true;
            }

            return false;
        }

        public static List<string> GetBlobNames(string connectionString, string containerName)
        {
            CheckIsNotNullOrWhitespace(nameof(connectionString), connectionString);
            CheckIsNotNullOrWhitespace(nameof(containerName), containerName);

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
            CheckIsNotNullOrWhitespace(nameof(connectionString), connectionString);
            CheckIsNotNullOrWhitespace(nameof(containerName), containerName);

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
