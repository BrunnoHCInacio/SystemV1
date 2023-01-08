using FluentValidation.Results;
using Moq.AutoMock;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Entitys;
using Xunit;

namespace SystemV1.Domain.Test.ValidationTests
{
    public class ValidationTestBase
    {
        protected readonly AutoMocker _autoMocker;

        public ValidationTestBase(AutoMocker autoMocker)
        {
            _autoMocker = autoMocker;
        }

        protected void VerifyResult(ValidationResult result, bool isValidResult)
        {
            Assert.NotNull(result);
            switch (isValidResult)
            {
                case true:
                    Assert.True(result.IsValid);
                    break;

                case false:
                    Assert.False(result.IsValid);
                    Assert.NotEmpty(result.Errors);
                    break;
            }
        }

        protected void SetSetupMock<TEntity, TRepository>(Expression<Func<TEntity, bool>> condition,
                                                          bool exists)
            where TRepository : class, IRepository<TEntity>
            where TEntity : Entity
        {
            _autoMocker.GetMock<TRepository>()
                            .Setup(r => r.ExistsAsync(condition))
                            .Returns(Task.FromResult(exists));
        }
    }
}