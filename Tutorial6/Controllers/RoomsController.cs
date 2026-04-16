using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tutorial6.DTOs;
using Tutorial6.Helpers;
using Tutorial6.Models;

namespace Tutorial6.Controllers

    
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        [Route("{id}")]
        [HttpGet]
        public IActionResult GetById([FromRoute] int id)
        {
            var room = DataStore.Rooms.FirstOrDefault(x => x.Id == id);
            if (room == null)
                return NotFound("There is not room with id: " + id);
            return Ok(room);
        }

        [Route("building/{buildingCode}")]
        [HttpGet]
        public IActionResult GetByBuilding([FromRoute] string buildingCode)
        {
            var rooms = DataStore.Rooms
                .Where(x => x.BuildingCode.Equals(buildingCode, StringComparison.OrdinalIgnoreCase))
                .ToList();
            if (rooms.Count == 0)
                return NotFound("There are no rooms in the building: " + buildingCode);
            return Ok(rooms);
        }

        
        [HttpGet]
        public IActionResult GetRooms([FromQuery] int? minCapacity, [FromQuery] bool? hasProjector, [FromQuery] bool? activeOnly)
        {
            var filteredRooms = DataStore.Rooms.AsQueryable();
            
            if (minCapacity.HasValue)
            {
                filteredRooms = filteredRooms.Where(r => r.Capacity >= minCapacity.Value);
            }

            if (hasProjector.HasValue)
            {
                filteredRooms = filteredRooms.Where(r => r.HasProjector == hasProjector.Value);
            }

            if (activeOnly.HasValue && activeOnly.Value)
            {
                filteredRooms = filteredRooms.Where(r => r.IsActive == true);
            }
            
            return Ok(filteredRooms.ToList());
        }


        [HttpPost]
        public IActionResult Post([FromBody] CreateRoomDto createRoomDto)
        {
            var room = new Room()
            {
                Id = DataStore.Rooms.Max(r => r.Id) + 1,
                Name = createRoomDto.Name,
                Capacity = createRoomDto.Capacity,
                BuildingCode = createRoomDto.BuildingCode,
                Floor = createRoomDto.Floor,
                HasProjector = createRoomDto.HasProjector,
                IsActive = createRoomDto.IsActive,
            };
            
            DataStore.Rooms.Add(room);
            return CreatedAtAction(nameof(GetById), new { Id = room.Id }, room);
        }

        [Route("{id}")]
        [HttpPut]
        public IActionResult UpdateRoom(int id, [FromBody] Room updatedRoom)
        {

            var existingRoom = DataStore.Rooms.FirstOrDefault(r => r.Id == id);
            if (existingRoom == null)
            {
                return NotFound($"Room with ID {id} not found.");
            }
            
            existingRoom.Name = updatedRoom.Name;
            existingRoom.BuildingCode = updatedRoom.BuildingCode;
            existingRoom.Floor = updatedRoom.Floor;
            existingRoom.Capacity = updatedRoom.Capacity;
            existingRoom.HasProjector = updatedRoom.HasProjector;
            existingRoom.IsActive = updatedRoom.IsActive;

            return Ok(existingRoom);
        }
        
        [Route("{id}")]
        [HttpDelete]
        public IActionResult DeleteRoom([FromRoute] int id)
        {
            var room = DataStore.Rooms.FirstOrDefault(x => x.Id == id);

            if (room == null)
            {
                return NotFound("There is no room wiht id: " + id);
            }
            var hasReservations = DataStore.Reservations.Any(res => res.RoomId == id);
            if (hasReservations)
                return Conflict("Cannot delete room because it has reservations.");

            DataStore.Rooms.Remove(room);
            return NoContent();

        }
    }
}
