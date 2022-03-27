using Newtonsoft.Json;
using System.Collections.Generic;
using SystemV1.Application.ViewModels;

namespace SystemV1.Domain.Test.IntegrationTest
{
    public class StateDeserialize
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("errors")]
        public List<string> Errors { get; set; }

        [JsonProperty("data")]
        public StateViewModel Data { get; set; }
    }
}