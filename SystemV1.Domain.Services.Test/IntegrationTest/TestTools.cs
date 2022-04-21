using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SystemV1.Domain.Test.IntegrationTest
{
    public class TestTools
    {
        public static TEntity GetRandomEntityInList<TEntity>(List<TEntity> entities)
        {
            if (entities.Count ==1) return entities.FirstOrDefault();

            var index = new Random().Next(1, entities.Count);
            var stateViewModel = entities.ElementAt(index);
            return stateViewModel;
        }

        public static async Task<TObject> DeserializeResponseAsync<TObject>(HttpResponseMessage response)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TObject>(jsonResponse);
        }
    }
}