using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DeckOfCards
{
    public class CheckStatusResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("statusQueryGetUri")]
        public string StatusQueryGetUri { get; set; }
        [JsonPropertyName("sendEventPostUri")]
        public string SendEventPostUri { get; set; }
        [JsonPropertyName("terminatePostUri")]
        public string TerminatePostUri { get; set; }
        [JsonPropertyName("purgeHistoryDeleteUri")]
        public string PurgeHistoryDeleteUri { get; set; }
    }
}
