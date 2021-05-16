using System;
using SystemV1.Domain.Entitys;
using Xunit;

namespace SystemV1.Domain.Test
{
    public class StateDomainTest
    {
        [Fact]
        public void State_NewState_ReturnFillDomain()
        {
            var stateExpected = new
            {
                Id = Guid.NewGuid(),
                Name = "Goias"
            };

            var state = new State(stateExpected.Id, stateExpected.Name);

            Assert.Equal(stateExpected.Id, state.Id);
            Assert.Equal(stateExpected.Name, state.Name);
        }

        [Theory]
        [InlineData("Manaus")]
        [InlineData("Amazonas")]
        [InlineData("Minas Gerais")]
        [InlineData("Rio de Janeiro")]
        public void State_NewState_MultipleVerifyDomainState(string stateName)
        {
            //Arrange
            var stateExpected = new
            {
                Id = Guid.NewGuid(),
                Name = stateName
            };

            //Act
            var state = new State(stateExpected.Id, stateExpected.Name);

            //Assert
            Assert.Equal(stateExpected.Id, state.Id);
            Assert.Equal(stateExpected.Name, state.Name);
        }
    }
}