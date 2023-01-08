using FluentAssertions;
using Moq;
using Moq.AutoMock;
using System;
using System.Linq;
using System.Threading.Tasks;
using SystemV1.Application;
using SystemV1.Application.Interfaces.Mapper;
using SystemV1.Application.ViewModels;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Core.Interfaces.Services;
using SystemV1.Domain.Core.Interfaces.Uow;
using SystemV1.Domain.Core.Interfaces.Validations;
using SystemV1.Domain.Entitys;
using SystemV1.Domain.Services;
using SystemV1.Domain.Services.Notifications;
using SystemV1.Domain.Services.Test.Fixture;
using SystemV1.Domain.Services.Validations;
using SystemV1.Domain.Test.Fixture;
using Xunit;

namespace SystemV1.Domain.Test.ServiceTests
{
    [Collection(nameof(StateCollection))]
    public class StateServiceTest : ServiceTestBase
    {
        private readonly StateTestFixture _stateTestFixture;

        public StateServiceTest(StateTestFixture stateTestFixture) : base(new AutoMocker())
        {
            _stateTestFixture = stateTestFixture;
        }

        #region Função de adicionar

        [Fact(DisplayName = "Add state with success")]
        [Trait("UnitTests - Services", "State")]
        public async Task StateService_AddNewStateService_ShouldHaveSuccess()
        {
            //Arrange
            var countryFixture = new CountryTestFixture();
            var state = _stateTestFixture.GenerateValidState(countryFixture.GenerateValidCountry());
            SetSetupMock<State, IValidationState>(state);
            var stateService = _autoMocker.CreateInstance<ServiceState>();

            //Act
            await stateService.AddAsyncUow(state);

            //Assert
            _autoMocker.GetMock<IRepositoryState>().Verify(r => r.Add(state), Times.Once);
            _autoMocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Add state with fail")]
        [Trait("UnitTests - Services", "State")]
        public async Task StateService_AddNewStateService_ShouldntHaveSuccess()
        {
            //Arrange
            var state = _stateTestFixture.GenerateInvalidState();
            var errors = GenerateMockErrors("Name", ValidationState.StateNameRequired);
            SetSetupMock<State, IValidationState>(state, errors);
            var stateService = _autoMocker.CreateInstance<ServiceState>();

            //Act
            await stateService.AddAsyncUow(state);

            //Asert
            _autoMocker.GetMock<IRepositoryState>().Verify(r => r.Add(state), Times.Never);
            _autoMocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Never);
            _autoMocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Once);
        }

        [Fact(DisplayName = "Add state with success")]
        [Trait("UnitTests - Services", "State")]
        public async Task StateApplication_AddNewStateService_ShouldHasSuccess()
        {
            //Arrange
            var stateViewModel = _stateTestFixture.GenerateValidStateViewModel();

            var applicationServiceState = _autoMocker.CreateInstance<ApplicationServiceState>();

            //Act
            await applicationServiceState.AddAsync(stateViewModel);

            //Assert
            _autoMocker.GetMock<IServiceState>().Verify(s => s.AddAsyncUow(It.IsAny<State>()), Times.Once);
            _autoMocker.GetMock<IMapperState>().Verify(m => m.ViewModelToEntity(It.IsAny<StateViewModel>()), Times.Once);
        }

        #endregion Função de adicionar

        #region Função de modificar

        [Fact(DisplayName = "Update state with success")]
        [Trait("UnitTests - Services", "State")]
        public async Task StateService_UpdateStateService_ShouldHasSuccess()
        {
            //Arrange
            var countryFixture = new CountryTestFixture();
            var state = _stateTestFixture.GenerateValidState(countryFixture.GenerateValidCountry());
            SetSetupMock<State, IValidationState>(state);
            var stateService = _autoMocker.CreateInstance<ServiceState>();

            //Act
            await stateService.UpdateAsyncUow(state);

            //Assert
            _autoMocker.GetMock<IRepositoryState>().Verify(r => r.Update(state), Times.Once);
            _autoMocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Once);
            _autoMocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Never);
        }

        [Fact(DisplayName = "Update state with fail")]
        [Trait("UnitTests - Services", "State")]
        public async Task StateService_UdateStateService_ShouldFaied()
        {
            //Arrange
            var state = _stateTestFixture.GenerateInvalidState();
            var errors = GenerateMockErrors("Name", ValidationState.StateNameRequired);
            SetSetupMock<State, IValidationState>(state, errors);
            var stateService = _autoMocker.CreateInstance<ServiceState>();

            //Act
            await stateService.UpdateAsyncUow(state);

            //Assert
            _autoMocker.GetMock<IUnitOfWork>().Verify(s => s.CommitAsync(), Times.Never);
            _autoMocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Once);
        }

        [Fact(DisplayName = "Update state with success")]
        [Trait("UnitTests - Services", "State")]
        public async Task StateApplication_UpdateState_ShouldHasSuccess()
        {
            //Arrange

            var stateApplication = _autoMocker.CreateInstance<ApplicationServiceState>();
            var state = _stateTestFixture.GenerateValidStateViewModel();

            //Act
            await stateApplication.AddAsync(state);

            //Assert
            _autoMocker.GetMock<IServiceState>().Verify(s => s.AddAsyncUow(It.IsAny<State>()), Times.Once);
            _autoMocker.GetMock<IMapperState>().Verify(m => m.ViewModelToEntity(state), Times.Once);
        }

        #endregion Função de modificar

        #region Função obter

        [Fact(DisplayName = "Get all the states")]
        [Trait("UnitTests - Services", "State")]
        public async Task StateService_GetAll_ShouldHasSuccess()
        {
            //Arrange

            var page = 1;
            var pageSize = 10;
            _autoMocker.GetMock<IRepositoryState>()
                  .Setup(s => s.SearchAsync(null, page, pageSize))
                  .Returns(Task.FromResult(_stateTestFixture.GenerateStates(pageSize)));
            var stateService = _autoMocker.CreateInstance<ServiceState>();

            //Act
            var states = await stateService.SearchAsync(null, page, pageSize);

            //Assert
            states.Should().HaveCount(10);
        }

        [Fact(DisplayName = "Get state by id")]
        [Trait("UnitTests - Services", "State")]
        public async Task StateService_GetById_ShoudHasSuccess()
        {
            //Arrange

            var stateId = Guid.NewGuid();
            _autoMocker.GetMock<IRepositoryState>()
                  .Setup(r => r.GetEntityAsync(s => s.Id == stateId, null))
                  .Returns(Task.FromResult(_stateTestFixture.GenerateStates(1).FirstOrDefault()));
            var stateService = _autoMocker.CreateInstance<ServiceState>();

            //Act
            var state = await stateService.GetEntityAsync(s => s.Id == stateId, null);

            //Assert
            state.Should().NotBeNull();
        }

        [Fact(DisplayName = "Get state with country by id")]
        [Trait("UnitTests - Services", "State")]
        public async Task StateService_GetStateCountry_ShouldHasSuccess()
        {
            //Arrange
            var countryFixture = new CountryTestFixture();
            var country = countryFixture.GenerateValidCountry();
            _autoMocker.GetMock<IRepositoryState>()
                  .Setup(r => r.GetEntityAsync(c => c.Id == Guid.Empty, "Country"))
                  .Returns(Task.FromResult(_stateTestFixture.GenerateStates(quantity: 1, country: country).FirstOrDefault()));
            var stateService = _autoMocker.CreateInstance<ServiceState>();

            //Act
            var state = await stateService.GetEntityAsync(c => c.Id == Guid.Empty, "Country");

            //Asset
            state.Should().NotBeNull();
            state.Country.Should().NotBeNull();
        }

        [Fact(DisplayName = "Get state by name")]
        [Trait("UnitTests - Services", "State")]
        public async Task StateService_GetStateByName_ShouldHasSuccess()
        {
            //Arrange

            var name = "Aaaa";
            _autoMocker.GetMock<IRepositoryState>()
                  .Setup(r => r.SearchAsync(s => s.Name.ToUpper() == name.ToUpper()))
                  .Returns(Task.FromResult(_stateTestFixture.GenerateStates(3)));
            var stateService = _autoMocker.CreateInstance<ServiceState>();

            //Act
            var states = await stateService.SearchAsync(s => s.Name.ToUpper() == name.ToUpper());

            //Assert
            states.Should().HaveCount(3);
        }

        #endregion Função obter

        #region Função Remover

        [Fact(DisplayName = "Remove state with success")]
        [Trait("UnitTests - Services", "State")]
        public async Task StateService_Remove_ShouldHasSuccess()
        {
            //Arrange
            var state = _stateTestFixture.GenerateValidState();
            var stateService = _autoMocker.CreateInstance<ServiceState>();
            SetSetupMock<State, IValidationState>(state);

            //Act
            await stateService.RemoveAsyncUow(state);

            //Assert
            _autoMocker.GetMock<IRepositoryState>().Verify(s => s.Remove(It.IsAny<State>()), Times.Once);
            _autoMocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Remove state with references")]
        [Trait("UnitTests - Services", "State")]
        public async Task Remove_RemoveStateWithReferences_ShouldHasSuccess()
        {
            //Arrange
            var state = _stateTestFixture.GenerateValidState();
            var stateService = _autoMocker.CreateInstance<ServiceState>();
            var errors = GenerateMockErrors("", ValidationState.StateNotDeleteHaveLinks);
            SetSetupMock<State, IValidationState>(state, errors);

            //Act
            await stateService.RemoveAsyncUow(state);

            //Assert
            _autoMocker.GetMock<IRepositoryState>().Verify(s => s.Remove(state), Times.Never);
            _autoMocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Never);
        }

        #endregion Função Remover
    }
}