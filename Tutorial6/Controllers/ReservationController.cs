using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    }
}
