using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemV1.Domain.Test.IntegrationTest
{
    public class TestTools
    {
        public static TEntity GetEntityByRandom<TEntity>(List<TEntity> entities)
        {
            if (entities.Count ==1) return entities.FirstOrDefault();

            var index = new Random().Next(1, entities.Count);
            var stateViewModel = entities.ElementAt(index);
            return stateViewModel;
        }
    }
}