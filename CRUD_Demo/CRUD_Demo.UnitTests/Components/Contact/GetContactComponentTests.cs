using AutoMapper;

using CRUD_Demo.Business.Components.Contact;
using CRUD_Demo.Common.Models;
using CRUD_Demo.DataAccess.Repositories;
using CRUD_Demo.UnitTests.ObjectFactories.Contact;

using DataAccessCore.DataAccess.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ContactEntities = CRUD_Demo.DataAccess.Entities;

namespace CRUD_Demo.UnitTests.Components.Contact
{
    [TestClass]
    public class GetContactComponentTests
    {
        private Mock<IRepositoryFactory> _repoFactoryMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IContactRepository> _contactRepositoryMock;
        private IGetContactComponent _getContactComponent;

        [TestInitialize]
        public void TestInitialize()
        {
            _repoFactoryMock = new Mock<IRepositoryFactory>(MockBehavior.Default);
            _mapperMock = new Mock<IMapper>(MockBehavior.Default);
            _contactRepositoryMock = new Mock<IContactRepository>(MockBehavior.Default);

            _getContactComponent = new GetContactComponent(
                _repoFactoryMock.Object,
                _mapperMock.Object);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _repoFactoryMock.VerifyAll();
            _mapperMock.VerifyAll();
            _contactRepositoryMock.VerifyAll();
        }

        [TestMethod]
        public async Task GetAllAsync_ReturnSuccess_ReturnsContactModelList()
        {
            // Arrange
            IEnumerable<ContactEntities.Contact> contacts = ContactObjectFactory.GetEntityList();
            IEnumerable<ContactModel> contactModels = ContactObjectFactory.GetModelList();

            _repoFactoryMock
                .Setup(x => x.Create<IContactRepository>())
                .Returns(_contactRepositoryMock.Object);

            _contactRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(contacts);

            _mapperMock
                .Setup(x => x.Map<IEnumerable<ContactModel>>(contacts))
                .Returns(contactModels);

            //Act
           var result = await _getContactComponent.GetAllAsync();

            //Assert
            Assert.IsInstanceOfType(result, typeof(IEnumerable<ContactModel>));
        }
    }
}
