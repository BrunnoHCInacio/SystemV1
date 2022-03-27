using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json;
using SystemV1.Application.ViewModels;

namespace SystemV1.Domain.Test.IntegrationTest
{
    public class CountryDeserialize
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("data")]
        public CountryViewModel Data { get; set; }

        [JsonProperty("errors")]
        public List<string> Errors { get; set; }
    }
}