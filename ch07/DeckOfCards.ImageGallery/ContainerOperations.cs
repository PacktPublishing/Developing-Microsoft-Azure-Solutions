using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DeckOfCards.ImageGallery
{
    public static class ContainerOperations
    {
        public static bool Exists(string connectionString, string containerName)
        {
            var serviceClient = new BlobServiceClient(connectionString);
            var containerClient = serviceClient.GetBlobContainerClient(containerName);

            return containerClient.Exists();
        }

        public static void CreateContainer(string connectionString,
                                    string containerName,
                                    PublicAccessType publicAccessType = PublicAccessType.None)
        {
            var serviceClient = new BlobServiceClient(connectionString);
            var containerClient = serviceClient.GetBlobContainerClient(containerName);
            var response = containerClient.CreateIfNotExists(publicAccessType);

            CheckStatus(response);
        }

        private static void CheckStatus<T>(Azure.Response<T> response)
        {
            var httpResponse = response.GetRawResponse();
            var responseCode = (HttpStatusCode)httpResponse.Status;

            if (responseCode == HttpStatusCode.OK)
                return;

            throw new Exception($@"[{responseCode}] {httpResponse.ReasonPhrase}");
        }
    }
}
