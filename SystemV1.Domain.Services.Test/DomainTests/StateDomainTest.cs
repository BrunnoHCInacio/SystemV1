using System;
using System.Linq;
using SystemV1.Domain.Entitys;
using SystemV1.Domain.Services.Test.Fixture;
using SystemV1.Domain.Validations;
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

        [Fact]
        public void State_NewState_ReturnFillDomain()
        {
            var stateExpected = _stateTestFixture.GenerateStateValidExpected();

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

        [Fact(DisplayName = "Validate State Valid")]
        [Trait("Categoria", "Cadastro - Estado")]
        public void State_ValidateNewState_ShouldBeValid()
        {
            //Arrange
            var state = _stateTestFixture.GenerateValidState();

            //Act
            var result = state.ValidateState();

            //Assert
            Assert.True(result.IsValid);
            Assert.False(result.Errors.Any());
        }

        [Fact(DisplayName = "Validate State Invalid")]
        [Trait("Categoria", "Cadastro - Estado")]
        public void State_ValidateNewState_ShouldBeInvalid()
        {
            //Arrange
            var state = _stateTestFixture.GenerateInvalidState();

            //Act
            var result = state.ValidateState();

            //Assert
            Assert.False(result.IsValid);
            Assert.True(result.Errors.Any());
            Assert.Equal(2, result.Errors.Count);
            Assert.Contains(StateValidation.StateNameRequired, result.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(StateValidation.CountryRequired, result.Errors.Select(e => e.ErrorMessage));
        }
    }
}