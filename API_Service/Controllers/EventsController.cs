using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System.Data.SqlClient;
using API_Service.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {

        private readonly DBaseContext _context;

        public EventsController(DBaseContext context)
        {
            _context = context;
        }

        // GET: api/<EventsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventModel>>> GetAsync()
        {
            return await _context.EVENT_NAMES.ToListAsync();
        }

        // GET: api/<EventsController>/5
        [HttpGet("{eventName}")]
        public async Task<ActionResult<IEnumerable<ParticipantModel>>> GetAsync(string eventName)
        {
            var _event = await _context.EVENT_NAMES.FindAsync(eventName);

            if (_event == null) return NotFound();

            var delList = await _context.PARTICIPANTS.FromSqlRaw("SELECT * FROM dbo.PARTICIPANTS").Where(predicate: p => p.Event_Name == eventName).ToListAsync();

            return Ok(delList);
        }

        // POST api/<EventsController>
        [HttpPost("{eventName}")]
        public async Task<ActionResult<EventModel>> PostAsync(string eventName)
        {
            var new_event = new EventModel(eventName);

            await _context.EVENT_NAMES.AddAsync(new_event);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException)
            {
                if (EventExists(eventName)) return Conflict();

                else throw;
            }

            return Ok(new_event);
        }

        // PUT api/<EventsController>/5
        [HttpPut("{eventName}")]
        public async Task<ActionResult> PutAsync(string eventName, string fname, string lname, string email)
        {
            var _event = await _context.EVENT_NAMES.FindAsync(eventName);

            if (_event == null) return NotFound();

            if (_event.Participants_count == 25) return BadRequest();

            var _participant = new ParticipantModel(eventName, fname, lname, email);

            if (ParticipantExists(eventName, email)) return Conflict();

            else  await _context.PARTICIPANTS.AddAsync(_participant);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }

            _event.Participants_count += 1;

            _context.Entry(_event).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(eventName)) return NotFound();

                else throw;
            }

            return Ok();
        }

        // DELETE api/<EventsController>/5
        [HttpDelete("{eventName}")]
        public async Task<ActionResult> Delete(string eventName)
        {
            var _event = await _context.EVENT_NAMES.FindAsync(eventName);

            if (_event == null) return NotFound();

            _context.EVENT_NAMES.Remove(_event);

            var delList = await _context.PARTICIPANTS.FromSqlRaw("SELECT * FROM dbo.PARTICIPANTS").Where(predicate: p => p.Event_Name == eventName).ToListAsync();

            foreach (var part in delList)
                _context.PARTICIPANTS.Remove(part);

            await _context.SaveChangesAsync();

            return Ok();
        }

        private  bool EventExists(string name)
        {
            return _context.EVENT_NAMES.Any(e => e.Event_name == name);
        }

        private bool ParticipantExists(string eventName, string email)
        {
            return  _context.PARTICIPANTS.Any(p => p.Event_Name == eventName && p.EMail == email);
        }
    }
}
