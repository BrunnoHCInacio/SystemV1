using Moq.AutoMock;
using System;
using System.Threading.Tasks;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Services.Validations;
using SystemV1.Domain.Test.Fixture;
using Xunit;

namespace SystemV1.Domain.Test.ValidationTests
{
    [Collection(nameof(ProductCollection))]
    public class ProductValidationTests : ValidationTestBase
    {
        private readonly ProductTestFixture _productTestFixture;

        public ProductValidationTests(ProductTestFixture productTestFixture) : base(new AutoMocker())
        {
            _productTestFixture = productTestFixture;
        }

        [Fact(DisplayName = "Rules for add valid product")]
        [Trait("UnitTests - Validations", "Product")]
        public async Task ProductValidation_RulesForAdd_MustBeValid()
        {
            //Arrange
            var product = _productTestFixture.GenerateValidProduct(Guid.NewGuid());
            var productId = product.Id;
            _autoMocker.GetMock<IRepositoryProvider>()
                       .Setup(r => r.ExistsAsync(p => p.Id == productId))
                       .Returns(Task.FromResult(true));

            var validator = _autoMocker.CreateInstance<ValidationProduct>();

            //Act
            validator.RulesForAdd();
            var result = await validator.ValidateAsync(product);

            //Assert
            VerifyResult(result, true);
        }
    }
}