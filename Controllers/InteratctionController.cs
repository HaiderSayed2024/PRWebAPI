﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using PRWebAPI.Models;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace PRWebAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class InteratctionController : ControllerBase
    {
        private readonly PRContext _dbContext;

        public InteratctionController(PRContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Authorize]
        [HttpGet("GetInteratctionDetails")]
        public async Task<ActionResult<IEnumerable<InteractionDetails>>> GetInteratctionDetails()
        {
            if (_dbContext.tblInteractionDetails == null)
            {
                return NotFound();
            }
            return await _dbContext.tblInteractionDetails.ToListAsync();
        }

        [Authorize]
        [HttpGet("GetInteratctionDetailsByID")]
        public ActionResult<InteractionDetails> GetInteratctionDetailsByID(int id)
        {
            var objInteraction = _dbContext.tblInteractionDetails.FirstOrDefault(x => x.ID == id);
            if (objInteraction == null)
            {
                return NotFound();
            }           
            return Ok(objInteraction);
        }

        [Authorize]
        [HttpPost("PostInteratctionDetails")]
        public async Task<ActionResult<ContactDTO>> PostInteratctionDetailsDetails(InteractionDTO objInteraction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _dbContext.tblInteractionDetails.Add(new InteractionDetails
            {
                InteractionDate = objInteraction.InteractionDate,
                PatronName = objInteraction.PatronName,
                MeetingDate = objInteraction.MeetingDate,
                Reason = objInteraction.Reason,
                Comment = objInteraction.Comment               
            });
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(PostInteratctionDetailsDetails), new { InteractionDate = objInteraction.InteractionDate }, objInteraction);
        }

        [Authorize]
        [HttpPut("PutInteratctionDetails")]
        public async Task<ActionResult> PutInteratctionDetails(int id, InteractionDetails objInteraction)
        {
            if (id != objInteraction.ID)
            {
                return BadRequest();
            }
            _dbContext.tblInteractionDetails.Entry(objInteraction).State = EntityState.Modified;
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return Ok(objInteraction);

        }

        [Authorize]
        [HttpDelete("DeleteInteratctionDetails")]
        public async Task<ActionResult> DeleteInteratctionDetails(int id)
        {
            if (_dbContext.tblInteractionDetails == null)
            {
                return NotFound();
            }
            var objInteraction = await _dbContext.tblInteractionDetails.FindAsync(id);
            if (objInteraction == null)
            {
                return NotFound();
            }
            _dbContext.tblInteractionDetails.Remove(objInteraction);
            await _dbContext.SaveChangesAsync();
            return Ok(objInteraction);
        }

    }
}