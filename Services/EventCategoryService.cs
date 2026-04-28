using Eventix_Project.Data;
using Eventix_Project.DTOs.EventCategory;
using Eventix_Project.Models;
using Eventix_Project.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Eventix_Project.Services.Implementations;

public class EventCategoryService : IEventCategoryService
{
    private readonly AppDbContext _context;

    public EventCategoryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<EventCategoryDto>> GetAllAsync()
    {
        return await _context.EventCategories
            .Select(c => new EventCategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Icon = c.Icon
            })
            .ToListAsync();
    }

    public async Task<EventCategoryDto?> GetByIdAsync(int id)
    {
        var category = await _context.EventCategories.FindAsync(id);
        if (category == null) return null;

        return new EventCategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Icon = category.Icon
        };
    }

    public async Task<EventCategoryDto> CreateAsync(CreateEventCategoryDto dto)
    {
        var category = new EventCategory
        {
            Name = dto.Name,
            Icon = dto.Icon
        };

        _context.EventCategories.Add(category);
        await _context.SaveChangesAsync();

        return new EventCategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Icon = category.Icon
        };
    }

    public async Task<bool> UpdateAsync(int id, UpdateEventCategoryDto dto)
    {
        var category = await _context.EventCategories.FindAsync(id);
        if (category == null) return false;

        if (dto.Name != null) category.Name = dto.Name;
        if (dto.Icon != null) category.Icon = dto.Icon;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var category = await _context.EventCategories.FindAsync(id);
        if (category == null) return false;

        _context.EventCategories.Remove(category);
        await _context.SaveChangesAsync();
        return true;
    }
}