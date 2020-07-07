using Eintech.BusinessLayer;
using Eintech.DataLayer.Contracts;
using Eintech.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLayerTests
{
    [TestClass]
    [TestCategory("Business Layer")]
    public class PeopleDataServiceTest
    {
        private Mock<ICustomerRepository> repository;
        private Mock<ILogger<Customer>> logger;

        [TestInitialize]
        public void Initialize()
        {
            this.repository = new Mock<ICustomerRepository>();
            this.logger = new Mock<ILogger<Customer>>();

        }


        #region GetById
        [TestMethod]
        [TestProperty("GetById", "")]
        public async Task GetById_Success_Test()
        {
            // arrange
            Customer expected = new Customer { Id = 1, FirstName = "John", LastName = "McArthur" };
            this.repository.Setup(rep => rep.GetById(1)).ReturnsAsync(expected);

            CustomerDataService dataService = new CustomerDataService(this.repository.Object, this.logger.Object);

            // act
            Customer actual = await dataService.GetById(1);

            // assert
            Assert.IsInstanceOfType(actual, typeof(Customer));
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestProperty("GetById", "")]
        public async Task GetById_NotFound_Test()
        {
            // arrange
            Customer expected = null;
            this.repository.Setup(rep => rep.GetById(1)).ReturnsAsync(expected);

            CustomerDataService dataService = new CustomerDataService(this.repository.Object, this.logger.Object);

            // act
            Customer actual = await dataService.GetById(1);

            // assert
            Assert.IsNull(actual);
        }
        #endregion

        #region GetAll
        [TestMethod]
        [TestProperty("GetAll", "")]
        public void GetAll_Success_Test()
        {
            // arrange
            var expected = new List<Customer>
            {
               new Customer { Id = 1, FirstName = "John", LastName = "McArthur" },
               new Customer { Id = 2, FirstName = "John", LastName = "Smith" }
            }.AsQueryable();


            this.repository.Setup(rep => rep.GetAll()).Returns(expected);


            CustomerDataService dataService = new CustomerDataService(this.repository.Object, this.logger.Object);

            // act
            IQueryable<Customer> actual = dataService.GetAll();

            // assert
            Assert.IsInstanceOfType(actual, typeof(IQueryable<Customer>));
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(2, actual.Count());
        }

        [TestMethod]
        [TestProperty("GetAll", "")]
        public void GetAll_NoData_Test()
        {
            // arrange
            var expected = new List<Customer>().AsQueryable();

            this.repository.Setup(rep => rep.GetAll()).Returns(expected);

            CustomerDataService dataService = new CustomerDataService(this.repository.Object, this.logger.Object);

            // act
            IQueryable<Customer> actual = dataService.GetAll();

            // assert
            Assert.IsInstanceOfType(actual, typeof(IQueryable<Customer>));
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(0, actual.Count());
        }
        #endregion


        #region Add
        [TestMethod]
        [TestProperty("Add", "")]
        public async Task Add_Success_Test()
        {
            // arrange
            Customer newCustomer = new Customer { FirstName = "John", LastName = "McArthur" };
            Customer expected = new Customer { Id = 1, FirstName = "John", LastName = "McArthur" };
            this.repository.Setup(rep => rep.Add(newCustomer)).ReturnsAsync(expected);

            CustomerDataService dataService = new CustomerDataService(this.repository.Object, this.logger.Object);

            // act
            Customer actual = await dataService.Add(newCustomer);

            // assert
            Assert.IsInstanceOfType(actual, typeof(Customer));
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestProperty("Add", "")]
        [ExpectedException(typeof(DbUpdateException))]
        public async Task Add_Failure_Test()
        {
            // arrange
            Customer newCustomer = new Customer { FirstName = "John", LastName = "McArthur" };
            this.repository.Setup(rep => rep.Add(newCustomer)).ThrowsAsync(new DbUpdateException());

            CustomerDataService dataService = new CustomerDataService(this.repository.Object, this.logger.Object);

            // act
            Customer actual = await dataService.Add(newCustomer);

            // assert
        }

        #endregion


        #region Update
        [TestMethod]
        [TestProperty("Update", "")]
        public async Task Update_Success_Test()
        {
            // arrange
            Customer existingCustomer = new Customer { Id=1, FirstName = "John", LastName = "McArthur", LastUpdated = DateTime.Parse("2019/12/31 12:00:00") };
            Customer expected = new Customer { Id = 1, FirstName = "John", LastName = "McArthur", LastUpdated = DateTime.Now };
            this.repository.Setup(rep => rep.Update(existingCustomer)).ReturnsAsync(expected);

            CustomerDataService dataService = new CustomerDataService(this.repository.Object, this.logger.Object);

            // act
            Customer actual = await dataService.Update(existingCustomer);

            // assert
            Assert.IsInstanceOfType(actual, typeof(Customer));
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestProperty("Update", "")]
        [ExpectedException(typeof(DbUpdateException))]
        public async Task Update_Failure_Test()
        {
            // arrange
            Customer existingCustomer = new Customer { Id = 1, FirstName = "John", LastName = "McArthur", LastUpdated = DateTime.Parse("2019/12/31 12:00:00") };
            this.repository.Setup(rep => rep.Update(existingCustomer)).ThrowsAsync(new DbUpdateException());

            CustomerDataService dataService = new CustomerDataService(this.repository.Object, this.logger.Object);

            // act
            Customer actual = await dataService.Update(existingCustomer);

            // assert
        }

        #endregion

        #region delete
        [TestMethod]
        [TestProperty("Delete","")]
        public async Task Delete_Success_Test()
        {
            //arrange
            this.repository.Setup(rep => rep.DeleteById(1)).ReturnsAsync(true);
            CustomerDataService dataService = new CustomerDataService(this.repository.Object, this.logger.Object);

            //act
            bool actual = await dataService.Delete(1);

            // assert
            Assert.IsTrue(actual);

        }

        [TestMethod]
        [TestProperty("Delete", "")]
        [ExpectedException(typeof(DbUpdateException))]
        public async Task Delete_Failure_Test()
        {
            //arrange
            this.repository.Setup(rep => rep.DeleteById(1)).ThrowsAsync(new DbUpdateException());
            CustomerDataService dataService = new CustomerDataService(this.repository.Object, this.logger.Object);

            //act
            bool actual = await dataService.Delete(1);

            // assert

        }
        #endregion

    }
}
