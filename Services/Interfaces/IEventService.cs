using Eventix_Project.DTOs.Event;

namespace Eventix_Project.Services.Interfaces;

public interface IEventService
{
    Task<EventResponseDto> CreateAsync(CreateEventDto dto, int userId);
    Task<List<EventResponseDto>> GetAllAsync();
    Task<EventResponseDto?> GetByIdAsync(int id);
    Task<bool> UpdateAsync(int id, UpdateEventDto dto);
    Task<bool> DeleteAsync(int id);
}