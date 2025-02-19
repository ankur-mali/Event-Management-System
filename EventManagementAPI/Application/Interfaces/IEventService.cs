using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementAPI.Domain.Models;

namespace EventManagementAPI.Application.Interfaces
{
    public interface IEventService
    {
        Task<Event> CreateEventAsync(Event newEvent);
        Task<List<Event>> ListEventsAsync();
        Task<Event?> GetEventByIdAsync(int id);
        Task<bool> UpdateEventAsync(int id, Event updatedEvent);
        Task<bool> DeleteEventAsync(int id);
    }
}
