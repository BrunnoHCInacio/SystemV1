using Moq.AutoMock;
using System.Threading.Tasks;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Services.Validations;
using SystemV1.Domain.Test.Fixture;
using Xunit;

namespace SystemV1.Domain.Test.ValidationTests
{
    [Collection(nameof(CityCollection))]
    public class CityValidationTests : ValidationTestBase
    {
        private readonly CityTestFixture _cityTestFixture;

        public CityValidationTests(CityTestFixture cityTestFixture) : base(new AutoMocker())
        {
            _cityTestFixture = cityTestFixture;
        }

        [Fact(DisplayName = "Rules for add valid city")]
        [Trait("UnitTests - Validations", "City")]
        public void CityValidation_RulesForAdd_ShouldNotReturnFailure()
        {
            //Arrange
            var city = _cityTestFixture.GenerateValidCity();
            var stateId = city.State.Id;

            _autoMocker.GetMock<IRepositoryState>()
                       .Setup(r => r.ExistsAsync(s => s.Id == stateId))
                       .Returns(Task.FromResult(true));

            var validation = _autoMocker.CreateInstance<ValidationCity>();

            //Act
            validation.RulesForAdd();
            var result = validation.Validate(city);

            //Assert
            VerifyResult(result, true);
        }

        [Fact(DisplayName = "Rules for add invalid city")]
        [Trait("UnitTests - Validations", "City")]
        public void CityValidation_RulesForAdd_ShouldReturnFailure()
        {
            //Arrange
            var city = _cityTestFixture.GenerateInvalidCity();
            var stateId = city.StateId;

            _autoMocker.GetMock<IRepositoryState>()
                       .Setup(r => r.ExistsAsync(s => s.Id == stateId))
                       .Returns(Task.FromResult(false));

            var validation = _autoMocker.CreateInstance<ValidationCity>();

            //Act
            validation.RulesForAdd();
            var result = validation.Validate(city);

            //Assert
            VerifyResult(result, false);
        }

        [Fact(DisplayName = "Rules for update valid city")]
        [Trait("UnitTests - Validations", "City")]
        public void CityValidation_RulesForUpdate_ShouldNotReturnFailure()
        {
            //Arrange
            var city = _cityTestFixture.GenerateValidCity();
            var stateId = city.StateId;
            var cityId = city.Id;

            _autoMocker.GetMock<IRepositoryState>()
                       .Setup(r => r.ExistsAsync(s => s.Id == stateId))
                       .Returns(Task.FromResult(true));

            _autoMocker.GetMock<IRepositoryState>()
                       .Setup(r => r.ExistsAsync(s => s.Id == cityId))
                       .Returns(Task.FromResult(true));

            var validation = _autoMocker.CreateInstance<ValidationCity>();

            //Act
            validation.RulesForUpdate();
            var result = validation.Validate(city);

            //Assert
            VerifyResult(result, true);
        }

        [Fact(DisplayName = "Rules for update invalid city")]
        [Trait("UnitTests - Validations", "City")]
        public void CityValidation_RulesForUpdate_ShouldReturnFailure()
        {
            //Arrange
            var city = _cityTestFixture.GenerateValidCity();
            var stateId = city.State.Id;
            var cityId = city.Id;

            _autoMocker.GetMock<IRepositoryState>()
                       .Setup(r => r.ExistsAsync(s => s.Id == stateId))
                       .Returns(Task.FromResult(false));

            _autoMocker.GetMock<IRepositoryState>()
                       .Setup(r => r.ExistsAsync(s => s.Id == cityId))
                       .Returns(Task.FromResult(false));

            var validation = _autoMocker.CreateInstance<ValidationCity>();

            //Act
            validation.RulesForUpdate();
            var result = validation.Validate(city);

            //Assert
            VerifyResult(result, false);
        }
    }
}