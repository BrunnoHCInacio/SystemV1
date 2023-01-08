using Moq.AutoMock;
using System;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Entitys;
using SystemV1.Domain.Services.Test.Fixture;
using SystemV1.Domain.Services.Validations;
using Xunit;

namespace SystemV1.Domain.Test.ValidationTests
{
    [Collection(nameof(StateCollection))]
    public class StateValidationTests : ValidationTestBase
    {
        private readonly StateTestFixture _stateTestFixture;

        public StateValidationTests(StateTestFixture stateTestFixture) : base(new AutoMocker())
        {
            _stateTestFixture = stateTestFixture;
        }

        #region Function Add

        [Fact(DisplayName = "Rules for add valid state")]
        [Trait("UnitTests - Validations", "State")]
        public void ValidationState_RulesForAdd_ShouldNotReturnFailure()
        {
            //Arrange
            var country = new Country
            {
                Id = Guid.NewGuid()
            };
            var state = _stateTestFixture.GenerateValidState(country);

            SetSetupMock<Country, IRepositoryCountry>(c => c.Id == state.CountryId, true);
            var validation = _autoMocker.CreateInstance<ValidationState>();

            //Act
            validation.RulesForAdd();
            var result = validation.Validate(state);

            //Assert
            VerifyResult(result, true);
        }

        [Fact(DisplayName = "Rules for add invalid state")]
        [Trait("UnitTests - Validations", "State")]
        public void ValidationState_RulesForAdd_ShouldReturnFailure()
        {
            //Arrange
            var state = _stateTestFixture.GenerateInvalidState();

            SetSetupMock<Country, IRepositoryCountry>(c => c.Id == state.CountryId, false);
            var validation = _autoMocker.CreateInstance<ValidationState>();

            //Act
            validation.RulesForAdd();
            var result = validation.Validate(state);

            VerifyResult(result, false);
        }

        #endregion Function Add

        #region Function Update

        [Fact(DisplayName = "Rules for update valid state")]
        [Trait("UnitTests - Validations", "State")]
        public void ValidationState_RulesForUpdate_ShouldNotReturnFailure()
        {
            //Arrange
            var country = new Country
            {
                Id = Guid.NewGuid()
            };

            var state = _stateTestFixture.GenerateValidState(country);

            SetSetupMock<Country, IRepositoryCountry>(c => c.Id == state.CountryId, true);
            SetSetupMock<State, IRepositoryState>(s => s.Id == state.Id, true);
            var validation = _autoMocker.CreateInstance<ValidationState>();

            //Act
            validation.RulesForUpdate();
            var result = validation.Validate(state);

            VerifyResult(result, true);
        }

        [Fact(DisplayName = "Rules for update invalid state")]
        [Trait("UnitTests - Validations", "State")]
        public void ValidationState_RulesForUpdate_ShouldReturnFailure()
        {
            //Arrange
            var state = _stateTestFixture.GenerateInvalidState();

            SetSetupMock<State, IRepositoryState>(s => s.Id == state.Id, false);
            SetSetupMock<Country, IRepositoryCountry>(c => c.Id == state.CountryId, false);
            var validation = _autoMocker.CreateInstance<ValidationState>();

            //Act
            validation.RulesForAdd();
            var result = validation.Validate(state);

            VerifyResult(result, false);
        }

        [Fact(DisplayName = "Rules for update state with invalid id")]
        [Trait("UnitTests - Validations", "State")]
        public void ValidationState_RulesForUpdateStateWithInvalidId_ShouldReturnFailure()
        {
            //Arrange
            var country = new Country
            {
                Id = Guid.NewGuid()
            };

            var state = _stateTestFixture.GenerateValidState(country);

            SetSetupMock<State, IRepositoryState>(s => s.Id == state.Id, false);
            SetSetupMock<Country, IRepositoryCountry>(c => c.Id == state.CountryId, true);
            var validation = _autoMocker.CreateInstance<ValidationState>();

            //Act
            validation.RulesForUpdate();
            var result = validation.Validate(state);

            VerifyResult(result, false);
        }

        #endregion Function Update

        #region Function Delete

        [Fact(DisplayName = "Rules for delete state")]
        [Trait("UnitTests - Validations", "State")]
        public void ValidationState_RulesForDelete_ShouldNotReturnFailure()
        {
            //Arrange
            var country = new Country
            {
                Id = Guid.NewGuid()
            };

            var state = _stateTestFixture.GenerateValidState(country);

            SetSetupMock<City, IRepositoryCity>(c => c.StateId == state.Id, false);
            var validation = _autoMocker.CreateInstance<ValidationState>();

            //Act
            validation.RulesForDelete();
            var result = validation.Validate(state);

            //Assert
            VerifyResult(result, true);
        }

        [Fact(DisplayName = "Rules for delete state with reference of the city")]
        [Trait("UnitTests - Validations", "State")]
        public void ValidationState_RulesForDeleteWithReferences_ShouldNotReturnFailure()
        {
            //Arrange
            var country = new Country
            {
                Id = Guid.NewGuid()
            };

            var state = _stateTestFixture.GenerateValidState(country);

            SetSetupMock<City, IRepositoryCity>(c => c.StateId == state.Id, true);
            var validation = _autoMocker.CreateInstance<ValidationState>();

            //Act
            validation.RulesForDelete();
            var result = validation.Validate(state);

            //Assert
            VerifyResult(result, false);
        }

        #endregion Function Delete
    }
}