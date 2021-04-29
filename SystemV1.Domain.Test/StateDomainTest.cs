using System;
using SystemV1.Domain.Entitys;
using Xunit;

namespace SystemV1.Domain.Test
{
    public class StateDomainTest
    {
        [Fact]
        public void Create()
        {
            var stateExpected = new
            {
                Name = "Goias"
            };

            var state = new State(stateExpected.Name);

            Assert.Equal(stateExpected.Name, state.Name);
        }
    }
}