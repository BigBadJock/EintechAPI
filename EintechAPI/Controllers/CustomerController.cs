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
    [Route("api/customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerDataService customerDataService;
        private readonly ILogger<CustomerController> logger;

        public CustomerController(ICustomerDataService customerDataService, ILogger<CustomerController> logger)
        {
            this.customerDataService = customerDataService;
            this.logger = logger;
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Customer>> Get(int id)
        {
            try
            {
                Customer customer = await this.customerDataService.GetById(id);
                if(customer == null)
                {
                    return new NotFoundResult();
                }

                return Ok(customer);
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
        public ActionResult<Customer[]> GetAll()
        {
            try
            {
                IQueryable<Customer> customers = this.customerDataService.GetAll();

                return Ok(customers);
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
        public async Task<ActionResult> Create(Customer customer)
        {
            try
            {
                Guard.Against.NullOrWhiteSpace(customer.FirstName, "firstName");
                Guard.Against.NullOrWhiteSpace(customer.LastName, "lastName");

                var addedcustomer = await this.customerDataService.Add(customer);
                return CreatedAtAction(nameof(Get), new { id = addedcustomer.Id }, addedcustomer);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message, ex);
                throw ex;
            }
        }

        [HttpPut("{id:int}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Update(Customer customer)
        {
            try
            {
                var updated = await this.customerDataService.Update(customer);
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
                var success = await this.customerDataService.Delete(id);
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
