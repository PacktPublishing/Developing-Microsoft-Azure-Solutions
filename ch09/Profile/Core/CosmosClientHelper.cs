using System;
using System.Net;
using Microsoft.Azure.Cosmos;
using static Pineapple.Common.Preconditions;

namespace DeckOfCards
{
    public static class CosmosClientHelper
    {
        public static void EnsureSuccess(this DatabaseResponse databaseResponse)
        {
            CheckIsNotNull(nameof(databaseResponse), databaseResponse);

            if (databaseResponse.StatusCode == HttpStatusCode.Created ||
                databaseResponse.StatusCode == HttpStatusCode.OK)
                return;
            
            throw new Exception($"Database unavailable [{databaseResponse.StatusCode}]");
        }

        public static void EnsureSuccess(this ContainerResponse containerResponse)
        {
            CheckIsNotNull(nameof(containerResponse), containerResponse);

            if (containerResponse.StatusCode == HttpStatusCode.Created ||
                containerResponse.StatusCode == HttpStatusCode.OK)
                return;

            throw new Exception($"Container unavailable [{containerResponse.StatusCode}]");
        }

        public static void EnsureSuccess<T>(this ItemResponse<T> itemResponse)
        {
            CheckIsNotNull(nameof(itemResponse), itemResponse);

            if (itemResponse.StatusCode == HttpStatusCode.Created ||
                itemResponse.StatusCode == HttpStatusCode.OK)
                return;

            throw new Exception($"Itemnavailable [{itemResponse.StatusCode}]");
        }
    }
}
