using System;
using System.Linq;
using SystemV1.Application.ViewModels;
using SystemV1.Domain.Entitys;
using SystemV1.Domain.Services.Test.Fixture;
using Xunit;

namespace SystemV1.Domain.Test
{
    [Collection(nameof(StateCollection))]
    public class StateDomainTest
    {
        private readonly StateTestFixture _stateTestFixture;

        public StateDomainTest(StateTestFixture stateTestFixture)
        {
            _stateTestFixture = stateTestFixture;
        }

        [Fact(DisplayName = "Validate set as correct properties")]
        [Trait("UnitTests - Entity", "State")]
        public void State_NewState_ReturnFillDomain()
        {
            var stateExpected = _stateTestFixture.GenerateStateValidExpected();

            var state = new State(stateExpected.Id, stateExpected.Name);

            Assert.Equal(stateExpected.Id, state.Id);
            Assert.Equal(stateExpected.Name, state.Name);
        }

        [Fact(DisplayName = "Validate set as correct properties in view model")]
        [Trait("UnitTests - Entity", "State")]
        public void StateViewModel_NewState_ShouldSetCorrectProperties()
        {
            //Arrange
            var stateExpected = _stateTestFixture.GenerateStateValidExpected();

            //Act
            var stateViewModel = new StateViewModel
            {
                Id = stateExpected.Id,
                Name = stateExpected.Name
            };

            //Assert
            Assert.Equal(stateExpected.Id, stateViewModel.Id);
            Assert.Equal(stateExpected.Name, stateViewModel.Name);
        }

        [Theory]
        [InlineData("Manaus")]
        [InlineData("Amazonas")]
        [InlineData("Minas Gerais")]
        [InlineData("Rio de Janeiro")]
        [Trait("UnitTests - Entity", "State")]
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