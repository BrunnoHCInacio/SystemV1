using Moq;
using SystemV1.Domain.Core.Constants;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Core.Interfaces.Services;
using SystemV1.Domain.Core.Interfaces.Uow;
using SystemV1.Domain.Services;
using SystemV1.Domain.Services.Notifications;
using SystemV1.Domain.Services.Test.Fixture;
using SystemV1.Domain.Services.Validations;
using Xunit;

namespace SystemV1.Domain.Test.ServiceTests
{
    [Collection(nameof(StateCollection))]
    public class StateServiceTest
    {
        private readonly StateTestFixture _stateTestFixture;

        public StateServiceTest(StateTestFixture stateTestFixture)
        {
            _stateTestFixture = stateTestFixture;
        }

        [Fact(DisplayName = "Refatorar")]
        [Trait("Categoria", "Serviço - Estado")]
        public void State_ValidateNewState_ReturnTrueToValidateState()
        {
            ////Arrange
            //var state = _stateTestFixture.GenerateValidState();

            //var repositoryState = new Mock<IRepositoryState>();
            //var unitOfWork = new Mock<IUnitOfWork>();
            //var notifier = new Mock<INotifier>();
            //var stateService = new ServiceState(repositoryState.Object,
            //                                    unitOfWork.Object,
            //                                    notifier.Object);

            ////Act
            //var resultValidation = stateService.RunValidation(new StateValidation(), state);

            ////Assert
            //Assert.True(resultValidation);
        }

        [Fact(DisplayName = "Validar novo cadastro de estado inválido REFATORAR")]
        [Trait("Categoria", "Serviço - Estado")]
        public void State_ValidadeNewState_ReturnFalseToValidadeState()
        {
            //Arrange
            var state = _stateTestFixture.GenerateInvalidState();

            var repositoryState = new Mock<IRepositoryState>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var notifier = new Mock<INotifier>();

            var stateService = new ServiceState(repositoryState.Object,
                                                unitOfWork.Object,
                                                notifier.Object);

            //Act
            // var resultValidation = stateService.RunValidation(new StateValidation(), state);

            //Assert
            // Assert.False(resultValidation);
        }

        [Fact(DisplayName = "Validar notificações de novo cadastro de estado inválido")]
        [Trait("State", "Teste de validação de cadastro")]
        public void State_ValidadeNewState_ValidateMessagesNotificationFromValidationState()
        {
            //Arrange
            var state = _stateTestFixture.GenerateInvalidState();

            var repositoryState = new Mock<IRepositoryState>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var notifier = new Mock<INotifier>();

            var stateService = new ServiceState(repositoryState.Object,
                                                unitOfWork.Object,
                                                notifier.Object);

            //Act
            var resultValidation = stateService.RunValidation(state.ValidateState());

            //Assert
            Assert.False(resultValidation);

            repositoryState.Verify(r => r.Add(state), Times.Never);
            unitOfWork.Verify(u => u.CommitAsync(), Times.Never);
            notifier.Verify(n => n.Handle(new Notification("")), Times.Once);
        }

        [Fact(DisplayName = "Cadastrar novo estado REFATORAR")]
        [Trait("Categoria", "Serviço - Estado")]
        public async void State_NewState_ShouldHaveSuccess()
        {
            //Arrange
            var state = _stateTestFixture.GenerateValidState();

            var repositoryState = new Mock<IRepositoryState>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var notifier = new Mock<INotifier>();

            var stateService = new ServiceState(repositoryState.Object,
                                                unitOfWork.Object,
                                                notifier.Object);

            //Act
            //var resultValidation = stateService.RunValidation(new StateValidation(), state);
            await stateService.AddAsyncUow(state);

            //Asert
            //Assert.True(resultValidation);
            repositoryState.Verify(r => r.Add(state), Times.Once);
            unitOfWork.Verify(u => u.CommitAsync(), Times.Once);
            notifier.Verify(n => n.Handle(new Notification("")), Times.Never);
        }
    }
}