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
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var room = DataStore.Rooms.FirstOrDefault(x => x.Id == id);
            if (room == null)
                return NotFound($"There is no room with id: {id}");
            return Ok(room);
        }

        [HttpGet("building/{buildingCode}")]
        public IActionResult GetByBuilding(string buildingCode)
        {
            var rooms = DataStore.Rooms
                .Where(x => x.BuildingCode.Equals(buildingCode, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (rooms.Count == 0)
                return NotFound($"There are no rooms in the building: {buildingCode}");

            return Ok(rooms);
        }

        [HttpGet]
        public IActionResult GetRooms([FromQuery] int? minCapacity, [FromQuery] bool? hasProjector, [FromQuery] bool? activeOnly)
        {
            var query = DataStore.Rooms.AsQueryable();

            if (minCapacity.HasValue)
                query = query.Where(r => r.Capacity >= minCapacity.Value);

            if (hasProjector.HasValue)
                query = query.Where(r => r.HasProjector == hasProjector.Value);

            if (activeOnly.HasValue && activeOnly.Value)
                query = query.Where(r => r.IsActive);

            return Ok(query.ToList());
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateRoomDto createRoomDto)
        {
            int nextId = DataStore.Rooms.Any() ? DataStore.Rooms.Max(r => r.Id) + 1 : 1;

            var room = new Room()
            {
                Id = nextId,
                Name = createRoomDto.Name,
                Capacity = createRoomDto.Capacity,
                BuildingCode = createRoomDto.BuildingCode,
                Floor = createRoomDto.Floor,
                HasProjector = createRoomDto.HasProjector,
                IsActive = createRoomDto.IsActive,
            };

            DataStore.Rooms.Add(room);
            return CreatedAtAction(nameof(GetById), new { id = room.Id }, room);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateRoom(int id, [FromBody] Room updatedRoom)
        {
            var existingRoom = DataStore.Rooms.FirstOrDefault(r => r.Id == id);
            if (existingRoom == null)
                return NotFound($"Room with ID {id} not found.");

            existingRoom.Name = updatedRoom.Name;
            existingRoom.BuildingCode = updatedRoom.BuildingCode;
            existingRoom.Floor = updatedRoom.Floor;
            existingRoom.Capacity = updatedRoom.Capacity;
            existingRoom.HasProjector = updatedRoom.HasProjector;
            existingRoom.IsActive = updatedRoom.IsActive;

            return Ok(existingRoom);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteRoom(int id)
        {
            var room = DataStore.Rooms.FirstOrDefault(x => x.Id == id);
            if (room == null)
                return NotFound($"There is no room with id: {id}");

            var hasReservations = DataStore.Reservations.Any(res => res.RoomId == id);
            if (hasReservations)
                return Conflict("Cannot delete room because it has reservations.");

            DataStore.Rooms.Remove(room);
            return NoContent();
        }
    }
}