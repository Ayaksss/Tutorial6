using Microsoft.AspNetCore.Mvc;
using Tutorial6.DTOs;
using Tutorial6.Models;
using Tutorial6.Helpers;

namespace Tutorial6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get([FromQuery] DateTime? date, [FromQuery] string? status, [FromQuery] int? roomId)
        {
            var query = DataStore.Reservations.AsQueryable();

            if (date.HasValue)
                query = query.Where(r => r.Date.Date == date.Value.Date);

            if (!string.IsNullOrEmpty(status))
                query = query.Where(r => r.Status.Equals(status, StringComparison.OrdinalIgnoreCase));

            if (roomId.HasValue)
                query = query.Where(r => r.RoomId == roomId.Value);

            return Ok(query.ToList());
        }
        
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var res = DataStore.Reservations.FirstOrDefault(x => x.Id == id);
            if (res == null) return NotFound();
            return Ok(res);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Reservation res)
        {
            if (res.EndTime <= res.StartTime)
                return BadRequest("EndTime must be later than StartTime.");

            var room = DataStore.Rooms.FirstOrDefault(r => r.Id == res.RoomId);
            if (room == null) return NotFound("Room not found.");
            if (!room.IsActive) return BadRequest("Room is inactive.");

            bool isOverlapping = DataStore.Reservations.Any(r => 
                r.RoomId == res.RoomId && 
                r.Date.Date == res.Date.Date && 
                res.StartTime < r.EndTime && r.StartTime < res.EndTime);

            if (isOverlapping)
                return Conflict("Room is already reserved for this time.");

            res.Id = DataStore.Reservations.Any() ? DataStore.Reservations.Max(r => r.Id) + 1 : 1;
            DataStore.Reservations.Add(res);

            return CreatedAtAction(nameof(GetById), new { id = res.Id }, res);
        }
        
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Reservation updated)
        {
            var existing = DataStore.Reservations.FirstOrDefault(r => r.Id == id);
            if (existing == null) return NotFound();
            
            existing.RoomId = updated.RoomId;
            existing.OrganizerName = updated.OrganizerName;
            existing.Topic = updated.Topic;
            existing.Date = updated.Date;
            existing.StartTime = updated.StartTime;
            existing.EndTime = updated.EndTime;
            existing.Status = updated.Status;

            return Ok(existing);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var res = DataStore.Reservations.FirstOrDefault(x => x.Id == id);
            if (res == null) return NotFound();

            DataStore.Reservations.Remove(res);
            return NoContent();
        }
    }
}