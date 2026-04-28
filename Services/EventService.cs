using Eventix_Project.Data;
using Eventix_Project.DTOs.Event;
using Eventix_Project.Models;
using Eventix_Project.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Eventix_Project.Services.Implementations;

public class EventService : IEventService
{
    private readonly AppDbContext _context;

    public EventService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<EventResponseDto> CreateAsync(CreateEventDto dto, int userId)
    {
        string? imageUrl = dto.Image != null ? "uploaded-image-url" : null;

        var ev = new Event
        {
            Title = dto.Title,
            Description = dto.Description,
            Location = dto.Location,
            StartAt = dto.StartAt,
            EndAt = dto.EndAt,
            Capacity = dto.Capacity,
            CategoryId = dto.CategoryId,
            CommunityId = dto.CommunityId,
            Price = dto.Price,
            UserId = userId,
            Image = imageUrl,
            Status = "active"
        };

        _context.Events.Add(ev);
        await _context.SaveChangesAsync();

        return await Map(ev.Id);
    }

    public async Task<List<EventResponseDto>> GetAllAsync()
    {
        return await _context.Events
            .Include(e => e.Category)
            .Include(e => e.Community)
            .Select(e => new EventResponseDto
            {
                Id = e.Id,
                Title = e.Title,
                Location = e.Location,
                StartAt = e.StartAt,
                EndAt = e.EndAt,
                Price = e.Price,
                Capacity = e.Capacity,
                Status = e.Status,
                Image = e.Image,
                CategoryName = e.Category!.Name,
                CommunityName = e.Community!.Name
            })
            .ToListAsync();
    }

    public async Task<EventResponseDto?> GetByIdAsync(int id)
    {
        var ev = await _context.Events.FindAsync(id);
        if (ev == null) return null;

        return await Map(id);
    }

    public async Task<bool> UpdateAsync(int id, UpdateEventDto dto)
    {
        var ev = await _context.Events.FindAsync(id);
        if (ev == null) return false;

        if (dto.Title != null) ev.Title = dto.Title;
        if (dto.Description != null) ev.Description = dto.Description;
        if (dto.Location != null) ev.Location = dto.Location;
        if (dto.StartAt != null) ev.StartAt = dto.StartAt;
        if (dto.EndAt != null) ev.EndAt = dto.EndAt;
        if (dto.Capacity != null) ev.Capacity = dto.Capacity;
        if (dto.Price != null) ev.Price = dto.Price;
        if (dto.Status != null) ev.Status = dto.Status;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var ev = await _context.Events.FindAsync(id);
        if (ev == null) return false;

        _context.Events.Remove(ev);
        await _context.SaveChangesAsync();
        return true;
    }

    private async Task<EventResponseDto> Map(int id)
    {
        return await _context.Events
            .Where(e => e.Id == id)
            .Include(e => e.Category)
            .Include(e => e.Community)
            .Select(e => new EventResponseDto
            {
                Id = e.Id,
                Title = e.Title,
                Location = e.Location,
                StartAt = e.StartAt,
                EndAt = e.EndAt,
                Price = e.Price,
                Capacity = e.Capacity,
                Status = e.Status,
                Image = e.Image,
                CategoryName = e.Category!.Name,
                CommunityName = e.Community!.Name
            })
            .FirstAsync();
    }
}