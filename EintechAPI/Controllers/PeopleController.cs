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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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
                throw;
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
        public async Task<IActionResult> Create(Person person)
        {
            try
            {
                await this.peopleDataService.Add(person);
                return CreatedAtAction(nameof(Get), new { id = person.Id }, person);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message, ex);
                throw;
            }
        }

        [HttpPatch]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(Person person)
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
        public async Task<IActionResult> Delete(int id)
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
