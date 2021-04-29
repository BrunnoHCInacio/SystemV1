using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemV1.Domain.Services.Test.Enums;
using Xunit;

namespace SystemV1.Domain.Services.Test
{
    public static class GenericValidationTest
    {
        public static void RunValidation<TValidation, TEntity>(TValidation validation,
                                                               TEntity entity,
                                                               Case? caseValidate = null,
                                                               List<string> messagesExpected = null) where TValidation : AbstractValidator<TEntity>
        {
            var validator = validation.Validate(entity);

            if (caseValidate != null)
            {
                if (caseValidate == Case.Success)
                    Assert.True(validator.IsValid);
                if (caseValidate == Case.Fail)
                    Assert.False(validator.IsValid);

                return;
            }

            var errors = new List<string>();
            foreach (var erro in validator.Errors)
            {
                errors.Add(erro.ErrorMessage);
            }

            Assert.True(errors.Any());

            foreach (var error in errors)
            {
                Assert.True(errors.Count() == messagesExpected.Count());

                Assert.False(string.IsNullOrEmpty(error) && string.IsNullOrWhiteSpace(error));
                Assert.Contains(messagesExpected, m => m == error);
            }
        }
    }
}