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
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(DataStore.Rooms);
        }
        
        
        [Route("{id}")]
        [HttpGet]
        public IActionResult GetById([FromRoute] int id)
        {
            var room = DataStore.Rooms.FirstOrDefault(x => x.Id == id);
            if (room == null)
                return NotFound();
            return Ok(room);
        }

        [Route("building/{buildingCode}")]
        [HttpGet]
        public IActionResult GetByBuilding([FromRoute] string buildingcode)
        {
            var room = DataStore.Rooms.FirstOrDefault(x => x.BuildingCode == buildingcode);
            if (room == null)
                return NotFound();
            return Ok(room);
        }
        
        
        [HttpPost]
        public IActionResult Post([FromBody] CreateRoomDto createRoomDto)
        {
            var room = new Room()
            {
                Id = DataStore.Rooms.Count + 1,
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
  
    }
}
