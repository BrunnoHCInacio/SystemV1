using FluentAssertions;
using FluentValidation.Results;
using Moq;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemV1.Application;
using SystemV1.Application.Interfaces.Mapper;
using SystemV1.Application.ViewModels;
using SystemV1.Domain.Core.Constants;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Core.Interfaces.Services;
using SystemV1.Domain.Core.Interfaces.Uow;
using SystemV1.Domain.Entitys;
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

        #region Função de adicionar

        [Fact(DisplayName = "Add state with success")]
        [Trait("Categoria", "Serviço - Estado")]
        public async Task State_AddNewState_ShouldHaveSuccess()
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

        [Fact(DisplayName = "Add state with fail")]
        [Trait("Categoria", "Serviço - Estado")]
        public async Task State_AddNewState_ShouldntHaveSuccess()
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
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Exactly(1));
        }

        [Fact(DisplayName ="Add state with exception fail")]
        [Trait("Categoria", "Serviço - Estado")]
        public void State_NewStateWithException_SHouldFalied()
        {
            //Arrange 
            var state = _stateTestFixture.GenerateValidState();
            var mocker = new AutoMocker();
            
            mocker.GetMock<IRepositoryState>().Setup(s => s.Add(It.IsAny<State>())).Throws(new Exception());
            var stateService = mocker.CreateInstance<ServiceState>();

            //Act
            Func<Task> act = () => stateService.AddAsyncUow(state);

            //Assert
            mocker.GetMock<IUnitOfWork>().Verify(s => s.CommitAsync(), Times.Never);
            Assert.Throws<Exception>(() => stateService.Add(It.IsAny<State>()));
        }


        [Fact(DisplayName = "Add state with success")]
        [Trait("Categoria", "Estado - Aplicação")]
        public async Task StateApplication_AddNewState_ShouldHasSuccess()
        {
            //Arrange
            var stateViewModel = _stateTestFixture.GenerateValidStateViewModel();
            var mocker = new AutoMocker();
            var applicationServiceState = mocker.CreateInstance<ApplicationServiceState>();

            //Act
            await applicationServiceState.AddAsync(stateViewModel);

            //Assert
            mocker.GetMock<IServiceState>().Verify(s => s.AddAsyncUow(It.IsAny<State>()), Times.Once);
            mocker.GetMock<IMapperState>().Verify(m => m.ViewModelToEntity(It.IsAny<StateViewModel>()), Times.Once);

        }

        #endregion

        #region Função de modificar
        [Fact(DisplayName ="Update state with success")]
        [Trait("Categoria", "Estado - Serviço")]
        public async Task State_UpdateState_ShouldHasSuccess()
        {
            //Arrange
            var state = _stateTestFixture.GenerateValidState();
            var mocker = new AutoMocker();

            var stateService = mocker.CreateInstance<ServiceState>();
            mocker.GetMock<IUnitOfWork>().Setup(u => u.CommitAsync()).Returns(Task.FromResult(true));

            //Act
            await stateService.UpdateAsyncUow(state);

            //Assert
            mocker.GetMock<IRepositoryState>().Verify(r => r.Update(state), Times.Once);
            mocker.GetMock<IUnitOfWork>().Verify(u=>u.CommitAsync(), Times.Once);
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Never);
        }

        [Fact(DisplayName ="Update state with fail")]
        [Trait("Categoria", "Estado - Serviço")]
        public async Task State_UdateState_ShouldFaied() 
        {
            //Arrange
            var state = _stateTestFixture.GenerateInvalidState();
            var mocker = new AutoMocker();
            var stateService = mocker.CreateInstance<ServiceState>();

            //Act
            await stateService.UpdateAsyncUow(state);

            //Assert
            mocker.GetMock<IServiceState>().Verify(s => s.Update(state), Times.Never);
            mocker.GetMock<IUnitOfWork>().Verify(s => s.CommitAsync(), Times.Never);
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Once);
        }
        
        [Fact(DisplayName ="Update state with exception fail")]
        [Trait("Categoria", "Estado - Serviço")]
        public async Task State_UpdateWithException_ShouldFailed()
        {
            //Arrange
            var state = _stateTestFixture.GenerateValidState();
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryState>().Setup(r => r.Update(It.IsAny<State>())).Throws(new Exception());

            var stateService = mocker.CreateInstance<ServiceState>();

            //Act
            await stateService.UpdateAsyncUow(state);

            //Assert
            mocker.GetMock<IUnitOfWork>().Verify(u=>u.CommitAsync(), Times.Never);
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Once);
            Assert.Throws<Exception>(() => stateService.Update(It.IsAny<State>()));
        }
        #endregion

        #region Função obter
        [Fact(DisplayName ="Get all the states")]
        [Trait("Categoria", "Serviço - Estado")]
        public async Task StateService_GetAll_ShouldHasSuccess()
        {
            //Arrange
            var mocker = new AutoMocker();
            
            mocker.GetMock<IRepositoryState>()
                  .Setup(s => s.GetAllStatesAsync(It.IsAny<int>(), It.IsAny<int>()))
                  .Returns(Task.FromResult((IEnumerable<State>)_stateTestFixture.GenerateStates(10)));
            var stateService = mocker.CreateInstance<ServiceState>();

            //Act
            var states = await stateService.GetAllAsync(It.IsAny<int>(), It.IsAny<int>());

            //Assert
            states.Should().HaveCount(10);
            Assert.False(states.Count(s => !s.IsActive) > 0);
        }

        [Fact(DisplayName ="Get all states with exception")]
        [Trait("Categoria", "Estado - Serviço")]
        public async Task StateService_GetAllWithExceptionShouldFailed()
        {
            //Arrange
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryState>().Setup(r => r.GetAllStatesAsync(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception());
            var stateService = mocker.CreateInstance<ServiceState>();

            //Act
            var states = await stateService.GetAllAsync(It.IsAny<int>(), It.IsAny<int>());

            //Assert
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Once);
            states.Should().NotBeNull();
            Assert.False(states.Any());

        }

        [Fact(DisplayName ="Get state by id")]
        [Trait("Categoria","Serviço - Estado")]
        public async Task StateService_GetById_ShoudHasSuccess()
        {
            //Arrange
            var mocker = new AutoMocker();
            
            mocker.GetMock<IRepositoryState>()
                  .Setup(r => r.GetStateByIdAsync(It.IsAny<Guid>()))
                  .Returns(Task.FromResult(_stateTestFixture.GenerateStates(1).FirstOrDefault()));
            var stateService = mocker.CreateInstance<ServiceState>();

            //Act
            var state = await stateService.GetByIdAsync(It.IsAny<Guid>());

            //Assert
            state.Should().NotBeNull();
        }

        [Fact(DisplayName ="Get state by id with exception")]
        [Trait("Categoria", "Serviço - Estado")]
        public async Task StateService_GetByIdWithException_ShouldFailed()
        {
            //Arrange
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryState>()
                  .Setup(r => r.GetStateByIdAsync(It.IsAny<Guid>()))
                  .Throws(new Exception());
            var stateService = mocker.CreateInstance<ServiceState>();

            //Act
            var state = await stateService.GetByIdAsync(It.IsAny<Guid>());

            //Assert
            Assert.Null(state);
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Once);
        }

        [Fact(DisplayName ="Get state with country by id")]
        [Trait("Categoria", "Serviço - Estado")]
        public async Task StateService_GetStateCountry_ShouldHasSuccess()
        {
            //Arrange
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryState>()
                  .Setup(r => r.GetStateCountryByIdAsync(It.IsAny<Guid>()))
                  .Returns(Task.FromResult( _stateTestFixture.GenerateStates(quantity: 1, getRelationShip: true).FirstOrDefault()));
            var stateService = mocker.CreateInstance<ServiceState>();

            //Act
            var state = await stateService.GetStateCountryByIdAsync(It.IsAny<Guid>());

            //Asset
            state.Should().NotBeNull();
            state.Country.Should().NotBeNull();
        }

        [Fact(DisplayName ="Get state with country by id with exception")]
        [Trait("Categoria", "Serviço - Estado")]
        public async Task StateService_GetStateCountryWithException_ShouldFailed()
        {
            //Arrange
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryState>()
                  .Setup(r => r.GetStateCountryByIdAsync(It.IsAny<Guid>()))
                  .Throws(new Exception());
            var stateService = mocker.CreateInstance<ServiceState>();

            //Act
            var state = await stateService.GetStateCountryByIdAsync(It.IsAny<Guid>());

            //Assert
            Assert.Null(state);
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Once);
        }
        
        [Fact(DisplayName ="Get state by name")]
        [Trait("Categoria","Serviço - Estado")]
        public async Task StateService_GetStateByName_ShouldHasSuccess()
        {
            //Arrange
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryState>()
                  .Setup(r => r.GetByNameAsync(It.IsAny<string>()))
                  .Returns(Task.FromResult((IEnumerable<State>)_stateTestFixture.GenerateStates(3)));
            var stateService = mocker.CreateInstance<ServiceState>();

            //Act
            var states = await stateService.GetByNameAsync("Aaaa");

            //Assert
            states.Should().HaveCount(3);
        }

        [Fact(DisplayName ="Get state by empty name")]
        [Trait("Categoria", "Serviço - Estado")]
        public async Task StateService_GetStateByEmptyName_ShouldFailed()
        {
            //Arrange
            var mocker = new AutoMocker();
            var stateService = mocker.CreateInstance<ServiceState>();

            //Act
            var state = await stateService.GetByNameAsync(It.IsAny<string>());

            //Assert
            Assert.Null(state);
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Once);
        }

        [Fact(DisplayName = "Get state by name with exception")]
        [Trait("Categoria", "Serviço - Estado")]
        public async Task StateService_GetStateByNameWithException_ShouldFailed()
        {
            //Arrange
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryState>()
                  .Setup(r => r.GetByNameAsync(It.IsAny<string>()))
                  .Throws(new Exception());

            var stateService = mocker.CreateInstance<ServiceState>();

            //Act
            var state = await stateService.GetByNameAsync("name");

            //Assert
            Assert.Null(state);
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Once);
        }
        #endregion

        #region Função Remover

        [Fact(DisplayName ="Remove state with success")]
        [Trait("Categoria", "Serviço - Estado")]
        public async Task StateService_Remove_ShouldHasSuccess()
        {
            //Arrange
            var mocker = new AutoMocker();
            var stateService = mocker.CreateInstance<ServiceState>();
            var state = _stateTestFixture.GenerateValidState();

            //Act 
            await stateService.RemoveAsyncUow(state);

            //Assert
            mocker.GetMock<IServiceState>().Verify(s => s.Remove(It.IsAny<State>()), Times.Once);
            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Once);
            
        }

        [Fact(DisplayName ="Remove state with exception")]
        [Trait("Categoria", "Serviço - Estado")]
        public async Task StateService_RemoveWithException_ShouldFailed()
        {
            //Arrange
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryState>()
                  .Setup(r => r.Update(It.IsAny<State>()))
                  .Throws(new Exception());

            var stateService = mocker.CreateInstance<ServiceState>();
            var state = _stateTestFixture.GenerateValidState();

            //Act
            await stateService.RemoveAsyncUow(state);

            //Assert
            mocker.GetMock<INotifier>()
                  .Verify(n => n.Handle(It.IsAny<Notification>()),
                          Times.Once);
            Assert.Throws<Exception>(() => stateService.Remove(state));

        }
        #endregion
    }
}