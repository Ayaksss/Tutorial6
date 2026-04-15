using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tutorial6.DTOs;
using Tutorial6.Models;
using Tutorial6.Helpers;

namespace Tutorial6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        public IActionResult Get()
        {
            return Ok(DataStore.Reservations);
        }
        
        [Route("{id}")]
        [HttpGet]
        public IActionResult GetById([FromRoute] int id)
        {
            var reservation = DataStore.Reservations.FirstOrDefault(x => x.Id == id);
            if (reservation == null)
                return NotFound();
            return Ok(reservation);
        }
        
        [Route("filter")]
        [HttpGet]
        public IActionResult GetReservations([FromQuery] DateTime? date, [FromQuery] string? status, [FromQuery] int? roomId)
        {
            var query = DataStore.Reservations.AsQueryable();
            
            if (date.HasValue)
            {
                query = query.Where(r => r.Date.Date == date.Value.Date);
            }
            
            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(r => r.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
            }
            
            if (roomId.HasValue)
            {
                query = query.Where(r => r.RoomId == roomId.Value);
            }
    
            return Ok(query.ToList());
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateReservationDto createReservation)
        {
            var reservation = new Reservation()
            {
                RoomId = createReservation.RoomId,
                OrganizerName = createReservation.OrganizerName,
                Date = createReservation.Date,
                StartTime = createReservation.StartTime,
                EndTime = createReservation.EndTime,
                Status = createReservation.Status,
            };
            
            DataStore.Reservations.Add(reservation);
            return Ok("Created reservation with ID: " + reservation.Id);
        }
        
        
        [HttpPut]
        public IActionResult UpdateReservation(int id, [FromBody] Reservation updatedReservation)
        {

            var existingReservation = DataStore.Reservations.FirstOrDefault(r => r.Id == id);
            if (existingReservation == null)
            {
                return NotFound($"Room with ID {id} not found.");
            }
            
            existingReservation.RoomId = updatedReservation.RoomId;
            existingReservation.OrganizerName = updatedReservation.OrganizerName;
            existingReservation.Topic = updatedReservation.Topic;
            existingReservation.Date = updatedReservation.Date;
            existingReservation.StartTime = updatedReservation.StartTime;
            existingReservation.EndTime = updatedReservation.EndTime;
            existingReservation.Status = existingReservation.Status;

            return Ok(existingReservation);
        }

        [HttpDelete]
        public IActionResult DeleteReservation(int id)
        {
            var reservations = DataStore.Reservations.FirstOrDefault(x => x.Id == id);
            
            DataStore.Reservations.Remove(reservations);
            return Ok("Deleted reservation with ID: " + id);
        }
    }
}
