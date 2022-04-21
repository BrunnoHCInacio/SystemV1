using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json;
using SystemV1.Application.ViewModels;

namespace SystemV1.Domain.Test.IntegrationTest
{
    public class DeserializeList<TEntity>
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("data")]
        public List<TEntity> Data { get; set; }

        [JsonProperty("errors")]
        public List<string> Errors { get; set; }
    }
}