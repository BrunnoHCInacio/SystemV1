using System;
using System.Collections.Generic;
using SystemV1.Domain.Core.Constants;
using SystemV1.Domain.Entitys;
using SystemV1.Domain.Services.Test.Enums;
using SystemV1.Domain.Services.Validations;
using Xunit;

namespace SystemV1.Domain.Services.Test
{
    public class StateServiceTest
    {
        [Fact]
        public void TestValidations()
        {
            ValidateTest(Case.Success);
            ValidateTest(Case.Fail);
            ValidateItensValidation();
        }

        [Fact]
        public void Create()
        {
        }

        private void ValidateTest(Case caseValidate)
        {
            var state = new State("Goias");

            GenericValidationTest.RunValidation(new StateValidation(),
                                                state,
                                                caseValidate);
        }

        private void ValidateItem(string stateName,
                                             List<string> messagesExpected = null)
        {
            var state = new State(stateName);

            GenericValidationTest.RunValidation(new StateValidation(),
                                                state,
                                                null,
                                                messagesExpected);
        }

        private void ValidateItensValidation()
        {
            ValidateItem("Goias");
            var messages = new List<string> { ConstantsStateMessages.StateRequired };
            ValidateItem("", messages);
        }

        [Fact]
        public void Retreave()
        {
        }

        [Fact]
        public void Update()
        {
        }

        [Fact]
        public void Remove()
        {
        }
    }
}