using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SystemV1.Domain.Test.Fixture
{
    [CollectionDefinition(nameof(ProductItemCollection))]
    public class ProductItemCollection : ICollectionFixture<ProductItemFixture>
    {
    }

    public class ProductItemFixture : IDisposable
    {
        public void Dispose()
        {
        }
    }
}