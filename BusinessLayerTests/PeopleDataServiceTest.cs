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
        private Mock<IPeopleRepository> repository;
        private Mock<ILogger<Person>> logger;

        [TestInitialize]
        public void Initialize()
        {
            this.repository = new Mock<IPeopleRepository>();
            this.logger = new Mock<ILogger<Person>>();

        }


        #region GetById
        [TestMethod]
        [TestProperty("GetById", "")]
        public async Task GetById_Success_Test()
        {
            // arrange
            Person expected = new Person { Id = 1, FirstName = "John", LastName = "McArthur" };
            this.repository.Setup(rep => rep.GetById(1)).ReturnsAsync(expected);

            PeopleDataService dataService = new PeopleDataService(this.repository.Object, this.logger.Object);

            // act
            Person actual = await dataService.GetById(1);

            // assert
            Assert.IsInstanceOfType(actual, typeof(Person));
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestProperty("GetById", "")]
        public async Task GetById_NotFound_Test()
        {
            // arrange
            Person expected = null;
            this.repository.Setup(rep => rep.GetById(1)).ReturnsAsync(expected);

            PeopleDataService dataService = new PeopleDataService(this.repository.Object, this.logger.Object);

            // act
            Person actual = await dataService.GetById(1);

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
            var expected = new List<Person>
            {
               new Person { Id = 1, FirstName = "John", LastName = "McArthur" },
               new Person { Id = 2, FirstName = "John", LastName = "Smith" }
            }.AsQueryable();


            this.repository.Setup(rep => rep.GetAll()).Returns(expected);


            PeopleDataService dataService = new PeopleDataService(this.repository.Object, this.logger.Object);

            // act
            IQueryable<Person> actual = dataService.GetAll();

            // assert
            Assert.IsInstanceOfType(actual, typeof(IQueryable<Person>));
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(2, actual.Count());
        }

        [TestMethod]
        [TestProperty("GetAll", "")]
        public void GetAll_NoData_Test()
        {
            // arrange
            var expected = new List<Person>().AsQueryable();

            this.repository.Setup(rep => rep.GetAll()).Returns(expected);

            PeopleDataService dataService = new PeopleDataService(this.repository.Object, this.logger.Object);

            // act
            IQueryable<Person> actual = dataService.GetAll();

            // assert
            Assert.IsInstanceOfType(actual, typeof(IQueryable<Person>));
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
            Person newPerson = new Person { FirstName = "John", LastName = "McArthur" };
            Person expected = new Person { Id = 1, FirstName = "John", LastName = "McArthur" };
            this.repository.Setup(rep => rep.Add(newPerson)).ReturnsAsync(expected);

            PeopleDataService dataService = new PeopleDataService(this.repository.Object, this.logger.Object);

            // act
            Person actual = await dataService.Add(newPerson);

            // assert
            Assert.IsInstanceOfType(actual, typeof(Person));
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestProperty("Add", "")]
        [ExpectedException(typeof(DbUpdateException))]
        public async Task Add_Failure_Test()
        {
            // arrange
            Person newPerson = new Person { FirstName = "John", LastName = "McArthur" };
            this.repository.Setup(rep => rep.Add(newPerson)).ThrowsAsync(new DbUpdateException());

            PeopleDataService dataService = new PeopleDataService(this.repository.Object, this.logger.Object);

            // act
            Person actual = await dataService.Add(newPerson);

            // assert
        }

        #endregion


        #region Update
        [TestMethod]
        [TestProperty("Update", "")]
        public async Task Update_Success_Test()
        {
            // arrange
            Person existingPerson = new Person { Id=1, FirstName = "John", LastName = "McArthur", LastUpdated = DateTime.Parse("2019/12/31 12:00:00") };
            Person expected = new Person { Id = 1, FirstName = "John", LastName = "McArthur", LastUpdated = DateTime.Now };
            this.repository.Setup(rep => rep.Update(existingPerson)).ReturnsAsync(expected);

            PeopleDataService dataService = new PeopleDataService(this.repository.Object, this.logger.Object);

            // act
            Person actual = await dataService.Update(existingPerson);

            // assert
            Assert.IsInstanceOfType(actual, typeof(Person));
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestProperty("Update", "")]
        [ExpectedException(typeof(DbUpdateException))]
        public async Task Update_Failure_Test()
        {
            // arrange
            Person existingPerson = new Person { Id = 1, FirstName = "John", LastName = "McArthur", LastUpdated = DateTime.Parse("2019/12/31 12:00:00") };
            this.repository.Setup(rep => rep.Update(existingPerson)).ThrowsAsync(new DbUpdateException());

            PeopleDataService dataService = new PeopleDataService(this.repository.Object, this.logger.Object);

            // act
            Person actual = await dataService.Update(existingPerson);

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
            PeopleDataService dataService = new PeopleDataService(this.repository.Object, this.logger.Object);

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
            PeopleDataService dataService = new PeopleDataService(this.repository.Object, this.logger.Object);

            //act
            bool actual = await dataService.Delete(1);

            // assert

        }
        #endregion

    }
}
