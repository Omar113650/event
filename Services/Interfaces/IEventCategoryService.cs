using Eventix_Project.DTOs.EventCategory;

namespace Eventix_Project.Services.Interfaces;

public interface IEventCategoryService
{
    Task<List<EventCategoryDto>> GetAllAsync();
    Task<EventCategoryDto?> GetByIdAsync(int id);
    Task<EventCategoryDto> CreateAsync(CreateEventCategoryDto dto);
    Task<bool> UpdateAsync(int id, UpdateEventCategoryDto dto);
    Task<bool> DeleteAsync(int id);
}