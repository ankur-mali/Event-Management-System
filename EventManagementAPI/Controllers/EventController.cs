using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EventManagementAPI.Application.Interfaces;
using EventManagementAPI.Domain.Models;

namespace EventManagementAPI.Controllers
{
    [Route("api/events")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateEvent([FromBody] Event newEvent)
        {
            var createdEvent = await _eventService.CreateEventAsync(newEvent);
            return Ok(createdEvent);
        }

        [HttpGet("list")]
        public async Task<IActionResult> ListEvents()
        {
            var events = await _eventService.ListEventsAsync();
            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventById(int id)
        {
            var eventItem = await _eventService.GetEventByIdAsync(id);
            if (eventItem == null)
                return NotFound(new { message = "Event not found" });
            return Ok(eventItem);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateEvent(int id, [FromBody] Event updatedEvent)
        {
            var result = await _eventService.UpdateEventAsync(id, updatedEvent);
            if (!result)
                return NotFound(new { message = "Event not found" });
            return Ok(new { message = "Event updated successfully" });
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var result = await _eventService.DeleteEventAsync(id);
            if (!result)
                return NotFound(new { message = "Event not found" });
            return Ok(new { message = "Event deleted successfully" });
        }
    }
}
