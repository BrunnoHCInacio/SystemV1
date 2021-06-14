using Moq;
using Moq.AutoMock;
using System.Threading.Tasks;
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

        [Fact(DisplayName = "Add state with success")]
        [Trait("Categoria", "Serviço - Estado")]
        public async Task State_NewState_ShouldHaveSuccess()
        {
            //Arrange
            var state = _stateTestFixture.GenerateValidState();
            var mocker = new AutoMocker();
            var stateService = mocker.CreateInstance<ServiceState>();
            mocker.GetMock<IUnitOfWork>().Setup(u => u.CommitAsync()).Returns(Task.FromResult(true));

            //Act
            await stateService.AddAsyncUow(state);

            //Assert
            mocker.GetMock<IRepositoryState>().Verify(r => r.Add(state), Times.Once);
            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Add state with don't success")]
        [Trait("Categoria", "Serviço - Estado")]
        public async Task State_NewState_ShouldntHaveSuccess()
        {
            //Arrange
            var state = _stateTestFixture.GenerateInvalidState();
            var mocker = new AutoMocker();
            var stateService = mocker.CreateInstance<ServiceState>();

            //Act
            await stateService.AddAsyncUow(state);

            //Asert
            mocker.GetMock<IRepositoryState>().Verify(r => r.Add(state), Times.Never);
            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Never);
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Exactly(2));
        }
    }
}