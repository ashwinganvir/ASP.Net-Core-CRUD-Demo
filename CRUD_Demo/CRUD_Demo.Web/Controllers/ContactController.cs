using CRUD_Demo.Business.Commands;
using CRUD_Demo.Business.Commands.Contact;
using CRUD_Demo.Business.Queries;
using CRUD_Demo.Business.Queries.Contact;
using CRUD_Demo.Common.Exceptions;
using CRUD_Demo.Common.Models;

using Microsoft.AspNetCore.Mvc;

using Serilog;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRUD_Demo.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly ICommandHandler _commandHandler;
        private readonly IQueryHandler _queryHandler;
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="commandHandler"></param>
        /// <param name="queryHandler"></param>
        /// <param name="logger"></param>
        public ContactController(
            ICommandHandler commandHandler,
            IQueryHandler queryHandler,
            ILogger logger)
        {
            _commandHandler = commandHandler;
            _queryHandler = queryHandler;
            _logger = logger;
        }


        // GET: api/<ContactController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var query = new GetContactQuery();

            IEnumerable<ContactModel> contactModels = await _queryHandler.HandleAsync<GetContactQuery, IEnumerable<ContactModel>>(query);

            return Ok(contactModels);
        }


        // POST api/<ContactController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ContactModel contact)
        {
            try
            {
                var command = new SaveContactCommand
                {
                    ContactModel = contact
                };

                ContactModel contactModel = await _commandHandler.HandleAsync<SaveContactCommand, ContactModel>(command);
                return Ok(contactModel);
            }
            catch (RecordNotFoundException ex)
            {
                _logger.Error(ex, "Failed to update Contact.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to update Contact.");
                return StatusCode(500);
            }

        }

        // PUT api/<ContactController>/5
        [HttpPut("{contactId}")]
        public async Task<IActionResult> Put(Guid contactId, [FromBody] ContactModel contact)
        {
            if (contactId != contact.ContactId)
            {
                return BadRequest("ContactId in model must match contactId in route");
            }

            try
            {
                var command = new SaveContactCommand
                {
                    ContactModel = contact
                };

                ContactModel contactModel = await _commandHandler.HandleAsync<SaveContactCommand, ContactModel>(command);
                return Ok(contactModel);
            }
            catch (RecordNotFoundException ex)
            {
                _logger.Error(ex, "Failed to update Contact.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to update Contact.");
                return StatusCode(500);
            }
        }

        // DELETE api/<ContactController>/5
        [HttpDelete("{contactId:Guid}")]
        public async Task<IActionResult> Delete(Guid contactId)
        {
            try
            {
                var command = new DeleteContactCommand
                {
                    ContactId = contactId
                };

                await _commandHandler.HandleAsync<DeleteContactCommand>(command);
                return Ok();
            }
            catch (InvalidContactIdException ex)
            {
                _logger.Error(ex, "Failed to delete Contact.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to delete Contact.");
                return StatusCode(500);
            }
        }
    }
}
