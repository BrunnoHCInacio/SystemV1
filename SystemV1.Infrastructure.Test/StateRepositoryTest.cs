using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Domain.Entitys;
using SystemV1.Infrastructure.Data;
using SystemV1.Infrastructure.Data.Repositorys;

namespace SystemV1.Infrastructure.Test
{
    internal class StateRepositoryTest
    {
        [Test]
        public void GetAll()
        {
        }

        [Test]
        public void GetById()
        {
        }

        [Test]
        public void Add()
        {
            var mockSet = new Mock<DbSet<State>>();
            var mockContext = new Mock<SqlContext>();

            mockContext.Setup(m => m.States).Returns(mockSet.Object);
            var stateRepository = new RepositoryState(mockContext.Object);

            var state = new State("Goiás");

            stateRepository.Add(state);
            mockSet.Verify(m => m.Add(It.IsAny<State>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [Test]
        public void Update()
        {
        }

        [Test]
        public void Remove()
        {
        }
    }
}