using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.Data.Sql;
using Microsoft.Data.SqlClient;
using System.Configuration;
using Microsoft.AspNetCore.Mvc;
using PRWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace PRWebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    public class ContactController : ControllerBase
    {      
        private readonly PRContext _dbContext;
    
        public ContactController(PRContext dbContext)
        {
            _dbContext = dbContext;           
        }

        [Authorize]
        [HttpGet("GetContactDetails")]       
        public async Task<ActionResult<IEnumerable<ContactDetails>>> GetContactDetails ()
        {
            if(_dbContext.tblContactDetails == null)
            {
                return NotFound();
            }
            return await _dbContext.tblContactDetails.ToListAsync();
        }

        [Authorize]
        [HttpGet("GetContactDetailsById{id}")]
        public ActionResult<ContactDetails> GetContactDetailsById(int id)
        {
            var objContact = _dbContext.tblContactDetails.FirstOrDefault(x => x.ContactDetailsID == id);
            if (objContact == null)
            {
                return BadRequest("User does not exists.");
            }
            else
            {
                return Ok(objContact);
            }
        }

        [Authorize]
        [HttpPost("PostContactDetails")]
        public async Task<ActionResult<ContactDTO>> PostContactDetails(ContactDTO objContact)
        {
           
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
                _dbContext.tblContactDetails.Add(new ContactDetails
                {
                    Name = objContact.Name,
                    Phone = objContact.Phone,
                    Address = objContact.Address,
                    Designation = objContact.Designation,
                    Priority = objContact.Priority,
                    IsAIM = objContact.IsAIM,
                    Relation = objContact.Relation,
                    ContactAddedBy = objContact.ContactAddedBy,
                    ContactOwnership = objContact.ContactOwnership,
                    Status = objContact.Status,
                    CreatedDate = DateTime.Now.Date,
                    ModifiedDate = DateTime.Now.Date
                });

                await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(PostContactDetails), new { Name = objContact.Name },objContact);

        }

        [Authorize]
        [HttpPut("PutContactDetails")]
        public async Task<ActionResult> PutContactDetails(int id, ContactDetails objContact)
        {
            if(id != objContact.ContactDetailsID)
            {
                return BadRequest();
            }
            _dbContext.tblContactDetails.Entry(objContact).State = EntityState.Modified;
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return Ok(objContact);
           
        }

        [Authorize]
        [HttpDelete("DeleteContactDetails")]
        public async Task<ActionResult> DeleteContactDetails(int id)
        {
            if (_dbContext.tblContactDetails == null)
            {
                return NotFound();
            }

            var objContact = await _dbContext.tblContactDetails.FindAsync(id);
            if (objContact == null)
            {
                return NotFound();
            }
            _dbContext.tblContactDetails.Remove(objContact);
            await _dbContext.SaveChangesAsync();

            return Ok(objContact);
        }


    }
}
