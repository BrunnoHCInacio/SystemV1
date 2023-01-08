using Moq.AutoMock;
using System.Threading.Tasks;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Entitys;
using SystemV1.Domain.Services.Validations;
using SystemV1.Domain.Test.Fixture;
using Xunit;

namespace SystemV1.Domain.Test.ValidationTests
{
    [Collection(nameof(CountryCollection))]
    public class CountryValidationTests : ValidationTestBase
    {
        private readonly CountryTestFixture _countryTestFixture;

        public CountryValidationTests(CountryTestFixture countryTestFixture) : base(new AutoMocker())
        {
            _countryTestFixture = countryTestFixture;
        }

        [Fact(DisplayName = "Rules for add valid country")]
        [Trait("UnitTests - Validations", "Country")]
        public void ValidationCountry_RulesForAdd_ShouldNotReturnFailure()
        {
            //Arrange
            var country = _countryTestFixture.GenerateValidCountry();
            var validation = _autoMocker.CreateInstance<ValidationCountry>();

            //Act
            validation.RulesForAdd();
            var result = validation.Validate(country);

            //Assert
            VerifyResult(result, true);
        }

        [Fact(DisplayName = "Rules for add invalid country")]
        [Trait("UnitTests - Validations", "Country")]
        public void ValidationCountry_RulesForAdd_ShouldReturnFailure()
        {
            //Arrange
            var country = _countryTestFixture.GenerateInvalidCountry();
            var validation = _autoMocker.CreateInstance<ValidationCountry>();

            //Act
            validation.RulesForAdd();
            var result = validation.Validate(country);

            //Assert
            VerifyResult(result, false);
        }

        [Fact(DisplayName = "Rules for update valid country")]
        [Trait("UnitTests - Validations", "Country")]
        public void ValidationCountry_RulesForUpdate_ShouldNotReturnFailure()
        {
            //Arrange
            var country = _countryTestFixture.GenerateValidCountry();
            var countryId = country.Id;

            _autoMocker.GetMock<IRepositoryCountry>()
                       .Setup(r => r.ExistsAsync(s => s.Id == countryId))
                       .Returns(Task.FromResult(true));

            var validation = _autoMocker.CreateInstance<ValidationCountry>();

            //Act
            validation.RulesForUpdate();
            var result = validation.Validate(country);

            //Assert
            VerifyResult(result, true);
        }

        [Fact(DisplayName = "Rules for update invalid country")]
        [Trait("UnitTests - Validations", "Country")]
        public void ValidationCountry_RulesForUpdate_ShouldReturnFailure()
        {
            //Arrange
            var country = _countryTestFixture.GenerateInvalidCountry();
            SetSetupMock<Country, IRepositoryCountry>(c => c.Id == country.Id, false);

            var validation = _autoMocker.CreateInstance<ValidationCountry>();

            //Act
            validation.RulesForUpdate();
            var result = validation.Validate(country);

            //Assert
            VerifyResult(result, false);
        }

        [Fact(DisplayName = "Rules for delete country with out references with state")]
        [Trait("UnitTests - Validations", "Country")]
        public void ValidationCountry_RulesForDelete_ShouldNotReturnFailure()
        {
            //Arrange
            var country = _countryTestFixture.GenerateValidCountry();
            var countryId = country.Id;

            _autoMocker.GetMock<IRepositoryState>()
                .Setup(r => r.ExistsAsync(s => s.CountryId == countryId))
                .Returns(Task.FromResult(true));

            var validation = _autoMocker.CreateInstance<ValidationCountry>();

            //Act
            validation.RulesForDelete();
            var result = validation.Validate(country);

            //Assert
            VerifyResult(result, false);
        }

        [Fact(DisplayName = "Rules for delete country with references from state")]
        [Trait("UnitTests - Validations", "Country")]
        public void ValidationCountry_RulesForDelete_ShouldReturnFailure()
        {
            //Arrange
            var country = _countryTestFixture.GenerateValidCountry();

            var validation = _autoMocker.CreateInstance<ValidationCountry>();
            SetSetupMock<State, IRepositoryState>(s => s.CountryId == country.Id, false);

            //Act
            validation.RulesForDelete();
            var result = validation.Validate(country);

            //Assert
            VerifyResult(result, true);
        }
    }
}