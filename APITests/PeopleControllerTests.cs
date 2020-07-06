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

        private Mock<IPeopleDataService> dataService;
        private Mock<ILogger<PeopleController>> logger;

        [TestInitialize]
        public void Initialize()
        {
            this.dataService = new Mock<IPeopleDataService>();
            this.logger = new Mock<ILogger<PeopleController>>();
        }

        #region GetById
        [TestMethod]
        [TestProperty("GetById", "")]
        public async Task GetById_Success_Test()
        {
            // arrange
            Person expected = new Person { Id = 1, FirstName = "John", LastName = "McArthur" };
            this.dataService.Setup(ds => ds.GetById(1)).ReturnsAsync(expected);

            PeopleController controller = new PeopleController(this.dataService.Object, this.logger.Object);

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
            Person expected = null;
            this.dataService.Setup(ds => ds.GetById(1)).ReturnsAsync(expected);

            PeopleController controller = new PeopleController(this.dataService.Object, this.logger.Object);

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

            PeopleController controller = new PeopleController(this.dataService.Object, this.logger.Object);

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
            var expected = new List<Person>
            {
               new Person { Id = 1, FirstName = "John", LastName = "McArthur" },
               new Person { Id = 2, FirstName = "John", LastName = "Smith" }
            }.AsQueryable();

            this.dataService.Setup(ds => ds.GetAll()).Returns(expected);

            PeopleController controller = new PeopleController(this.dataService.Object, this.logger.Object);

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
            var expected = new List<Person>().AsQueryable();

            this.dataService.Setup(ds => ds.GetAll()).Returns(expected);

            PeopleController controller = new PeopleController(this.dataService.Object, this.logger.Object);

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

            PeopleController controller = new PeopleController(this.dataService.Object, this.logger.Object);

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
            Person expected = new Person { Id = 1, FirstName = "John", LastName = "McArthur" };
            this.dataService.Setup(ds => ds.Add(It.IsAny<Person>())).ReturnsAsync(expected);

            PeopleController controller = new PeopleController(this.dataService.Object, this.logger.Object);

            // act
            var actionResult = await controller.Create("John", "McArthur") as CreatedAtActionResult;
            var actual = actionResult.Value as Person;

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
            Person newPerson = null;
            this.dataService.Setup(ds => ds.Add(It.IsAny<Person>())).ThrowsAsync(new Exception());

            PeopleController controller = new PeopleController(this.dataService.Object, this.logger.Object);

            // act
            var actionResult = await controller.Create("John", "McArthur") as CreatedAtActionResult;

        }
        #endregion

        #region Update
        [TestMethod]
        [TestProperty("Update", "")]
        public async Task Update_Success_Test()
        {
            // arrange
            Person update = new Person { Id = 1, FirstName = "John", LastName = "McArthur" };
            Person expected = new Person { Id = 1, FirstName = "John", LastName = "McArthur", LastUpdated= DateTime.Parse("2020/07/06 22:00") };
            this.dataService.Setup(ds => ds.Update(update)).ReturnsAsync(expected);

            PeopleController controller = new PeopleController(this.dataService.Object, this.logger.Object);

            // act
            var actionResult = await controller.Update(update) as OkObjectResult;
            var actual = actionResult.Value as Person;

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
            Person update = null;
            this.dataService.Setup(ds => ds.Update(update)).ThrowsAsync(new Exception());

            PeopleController controller = new PeopleController(this.dataService.Object, this.logger.Object);

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
            Person delete = new Person { Id = 1, FirstName = "John", LastName = "McArthur" };
            this.dataService.Setup(ds => ds.Delete(1)).ReturnsAsync(true);

            PeopleController controller = new PeopleController(this.dataService.Object, this.logger.Object);

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

            PeopleController controller = new PeopleController(this.dataService.Object, this.logger.Object);

            // act
            var actionResult = await controller.Delete(1);
        }
        #endregion


    }
}
