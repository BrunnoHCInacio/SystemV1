using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SystemV1.Domain.Test.Fixture;
using Xunit;

namespace SystemV1.Domain.Test.ServiceTests
{
    [Collection(nameof(ProductCollection))]
    public class ProductServiceTest
    {
        private readonly ProductTestFixture _productTestFixture;

        public ProductServiceTest(ProductTestFixture productTestFixture)
        {
            _productTestFixture = productTestFixture;
        }

        [Fact(DisplayName ="Add new valid product")]
        [Trait("Categoria", "Serviço - Produto")]
        public async Task ServiceProduct_NewProduct_ShouldHaveSuccess()
        {

        }
    }
}
