using Ardalis.GuardClauses;
using Eintech.BusinessLayer.Contracts;
using Eintech.DataModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace EintechAPI.Controllers
{
    [Route("api/people")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly IPeopleDataService peopleDataService;
        private readonly ILogger<PeopleController> logger;

        public PeopleController(IPeopleDataService peopleDataService, ILogger<PeopleController> logger)
        {
            this.peopleDataService = peopleDataService;
            this.logger = logger;
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Person>> Get(int id)
        {
            try
            {
                Person person = await this.peopleDataService.GetById(id);
                if(person == null)
                {
                    return new NotFoundResult();
                }

                return Ok(person);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message, ex);
                throw ex;
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Person[]> GetAll()
        {
            try
            {
                IQueryable<Person> people = this.peopleDataService.GetAll();

                return Ok(people);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message, ex);
                throw;
            }
        }


        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Create(string firstName, string lastName)
        {
            try
            {
                Guard.Against.NullOrWhiteSpace(firstName, "firstName");
                Guard.Against.NullOrWhiteSpace(lastName, "lastName");

                Person person = new Person { FirstName = firstName, LastName = lastName };
                    var addedPerson = await this.peopleDataService.Add(person);
                return CreatedAtAction(nameof(Get), new { id = addedPerson.Id }, addedPerson);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message, ex);
                throw ex;
            }
        }

        [HttpPatch]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Update(Person person)
        {
            try
            {
                var updated = await this.peopleDataService.Update(person);
                return Ok(updated);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message, ex);
                throw;
            }
        }

        [HttpDelete]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var success = await this.peopleDataService.Delete(id);
                return Ok(success);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message, ex);
                throw;
            }
        }

    }
}
