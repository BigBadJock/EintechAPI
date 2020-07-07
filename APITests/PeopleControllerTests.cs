using Eintech.BusinessLayer.Contracts;
using Eintech.DataModels;
using EintechAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace APITests
{
    [TestClass]
    [TestCategory("API Controllers")]
    public class PeopleControllerTests
    {

        private Mock<ICustomerDataService> dataService;
        private Mock<ILogger<CustomerController>> logger;

        [TestInitialize]
        public void Initialize()
        {
            this.dataService = new Mock<ICustomerDataService>();
            this.logger = new Mock<ILogger<CustomerController>>();
        }

        #region GetById
        [TestMethod]
        [TestProperty("GetById", "")]
        public async Task GetById_Success_Test()
        {
            // arrange
            Customer expected = new Customer { Id = 1, FirstName = "John", LastName = "McArthur" };
            this.dataService.Setup(ds => ds.GetById(1)).ReturnsAsync(expected);

            CustomerController controller = new CustomerController(this.dataService.Object, this.logger.Object);

            // act
            var actionResult = await controller.Get(1);
            var okResult = actionResult.Result as OkObjectResult;

            // assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual((int)HttpStatusCode.OK, okResult.StatusCode);
            Assert.AreEqual(expected, okResult.Value);
        }

        [TestMethod]
        [TestProperty("GetById", "")]
        public async Task GetById_NotFound_Test()
        {
            // arrange
            Customer expected = null;
            this.dataService.Setup(ds => ds.GetById(1)).ReturnsAsync(expected);

            CustomerController controller = new CustomerController(this.dataService.Object, this.logger.Object);

            // act
            var actionResult = await controller.Get(1);
            var actual = actionResult.Result as NotFoundResult;

            // assert
            Assert.IsNotNull(actual);
            Assert.AreEqual((int)HttpStatusCode.NotFound, actual.StatusCode);
        }

        [TestMethod]
        [TestProperty("GetById", "")]
        [ExpectedException(typeof(Exception))]
        public async Task GetById_InternalServer_Test()
        {
            // arrange
            this.dataService.Setup(ds => ds.GetById(1)).ThrowsAsync(new Exception());

            CustomerController controller = new CustomerController(this.dataService.Object, this.logger.Object);

            // act
            var actionResult = await controller.Get(1);

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

            this.dataService.Setup(ds => ds.GetAll()).Returns(expected);

            CustomerController controller = new CustomerController(this.dataService.Object, this.logger.Object);

            // act
            var actionResult = controller.GetAll();
            var okResult = actionResult.Result as OkObjectResult;

            // assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual((int)HttpStatusCode.OK, okResult.StatusCode);
            Assert.AreEqual(expected, okResult.Value);
        }

        [TestMethod]
        [TestProperty("GetAll", "")]
        public void GetAll_NoData_Success_Test()
        {
            // arrange
            var expected = new List<Customer>().AsQueryable();

            this.dataService.Setup(ds => ds.GetAll()).Returns(expected);

            CustomerController controller = new CustomerController(this.dataService.Object, this.logger.Object);

            // act
            var actionResult = controller.GetAll();
            var okResult = actionResult.Result as OkObjectResult;

            // assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual((int)HttpStatusCode.OK, okResult.StatusCode);
            Assert.AreEqual(expected, okResult.Value);
        }

        [TestMethod]
        [TestProperty("GetAll", "")]
        [ExpectedException(typeof(Exception))]
        public void GetAll_InternalServer_Test()
        {
            // arrange
            this.dataService.Setup(ds => ds.GetAll()).Throws(new Exception());

            CustomerController controller = new CustomerController(this.dataService.Object, this.logger.Object);

            // act
            var actionResult = controller.GetAll();

        }

        #endregion

        #region Create
        [TestMethod]
        [TestProperty("Create", "")]
        public async Task Create_Success_Test()
        {
            // arrange
            Customer newCustomer = new Customer { FirstName = "John", LastName = "McArthur" };
            Customer expected = new Customer { Id = 1, FirstName = "John", LastName = "McArthur" };
            this.dataService.Setup(ds => ds.Add(It.IsAny<Customer>())).ReturnsAsync(expected);

            CustomerController controller = new CustomerController(this.dataService.Object, this.logger.Object);

            // act
            var actionResult = await controller.Create(newCustomer) as CreatedAtActionResult;
            var actual = actionResult.Value as Customer;

            // assert
            Assert.IsNotNull(actionResult);
            Assert.AreEqual((int)HttpStatusCode.Created, actionResult.StatusCode);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestProperty("Create", "")]
        [ExpectedException(typeof(Exception))]
        public async Task Create_Failure_Test()
        {
            // arrange
            Customer newCustomer = new Customer { FirstName = "John", LastName = "McArthur" };
            this.dataService.Setup(ds => ds.Add(It.IsAny<Customer>())).ThrowsAsync(new Exception());

            CustomerController controller = new CustomerController(this.dataService.Object, this.logger.Object);

            // act
            var actionResult = await controller.Create(newCustomer) as CreatedAtActionResult;

        }
        #endregion

        #region Update
        [TestMethod]
        [TestProperty("Update", "")]
        public async Task Update_Success_Test()
        {
            // arrange
            Customer update = new Customer { Id = 1, FirstName = "John", LastName = "McArthur" };
            Customer expected = new Customer { Id = 1, FirstName = "John", LastName = "McArthur", LastUpdated= DateTime.Parse("2020/07/06 22:00") };
            this.dataService.Setup(ds => ds.Update(update)).ReturnsAsync(expected);

            CustomerController controller = new CustomerController(this.dataService.Object, this.logger.Object);

            // act
            var actionResult = await controller.Update(update) as OkObjectResult;
            var actual = actionResult.Value as Customer;

            // assert
            Assert.IsNotNull(actionResult);
            Assert.AreEqual((int)HttpStatusCode.OK, actionResult.StatusCode);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestProperty("Update", "")]
        [ExpectedException(typeof(Exception))]
        public async Task Update_Failure_Test()
        {
            // arrange
            Customer update = null;
            this.dataService.Setup(ds => ds.Update(update)).ThrowsAsync(new Exception());

            CustomerController controller = new CustomerController(this.dataService.Object, this.logger.Object);

            // act
            var actionResult = await controller.Update(update);
        }
        #endregion

        #region Delete
        [TestMethod]
        [TestProperty("Delete", "")]
        public async Task Delete_Success_Test()
        {
            // arrange
            Customer delete = new Customer { Id = 1, FirstName = "John", LastName = "McArthur" };
            this.dataService.Setup(ds => ds.Delete(1)).ReturnsAsync(true);

            CustomerController controller = new CustomerController(this.dataService.Object, this.logger.Object);

            // act
            var actionResult = await controller.Delete(1) as OkObjectResult;

            // assert
            Assert.IsNotNull(actionResult);
            Assert.AreEqual((int)HttpStatusCode.OK, actionResult.StatusCode);
        }

        [TestMethod]
        [TestProperty("Delete", "")]
        [ExpectedException(typeof(Exception))]
        public async Task Delete_Failure_Test()
        {
            // arrange
            this.dataService.Setup(ds => ds.Delete(1)).ThrowsAsync(new Exception());

            CustomerController controller = new CustomerController(this.dataService.Object, this.logger.Object);

            // act
            var actionResult = await controller.Delete(1);
        }
        #endregion


    }
}
