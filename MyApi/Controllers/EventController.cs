using Eventix_Project.Controllers;
using Eventix_Project.Data;
using Eventix_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventixAPI.Controllers;

[ApiController]
[Route("api/event")]
public class EventController : ControllerBase
{
    private readonly AppDbContext _context;

    public EventController(AppDbContext context)
    {
        _context = context;
    }

    // ================= TODAY =================
    [HttpGet("today")]
    public async Task<IActionResult> GetToday()
    {
        var today = DateTime.Today;

        var events = await _context.Events
            .Where(e => e.StartAt == today)
            .ToListAsync();

        return Ok(events);
    }

    // ================= SEARCH =================
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string q)
    {
        var events = await _context.Events
            .Where(e =>
                e.Title.Contains(q) ||
                e.Description!.Contains(q) ||
                e.Location.Contains(q))
            .ToListAsync();

        return Ok(events);
    }

    // ================= ASK (LLM MOCK) =================
    [HttpPost("ask")]
    public IActionResult Ask([FromBody] string prompt)
    {
        return Ok(new { answer = $"AI response for: {prompt}" });
    }

    // ================= CREATE EVENT =================
    [HttpPost("add-event")]
    [Authorize(Roles = "Admin,Organizer")]
    public async Task<IActionResult> Create([FromForm] CreateEventDto dto)
    {
        string? imageUrl = null;

        if (dto.File != null)
        {
            // مؤقت لحد ما تضيف Cloudinary
            imageUrl = "uploaded-url";
        }

        var ev = new Event
        {
            Title = dto.Title,
            Description = dto.Description,
            Location = dto.Location,
            StartAt = dto.StartAt,
            Price = dto.Price,
            UserId = dto.UserId,
            CategoryId = dto.CategoryId,
            Image = imageUrl
        };

        _context.Events.Add(ev);
        await _context.SaveChangesAsync();

        return Ok(ev);
    }

    // ================= GET EVENTS =================
    [HttpGet("get-event")]
    public async Task<IActionResult> GetAll([FromQuery] EventQueryDto query)
    {
        var data = _context.Events.AsQueryable();

        if (!string.IsNullOrEmpty(query.Search))
            data = data.Where(e => e.Title.Contains(query.Search));

        if (query.MinPrice != null && query.MaxPrice != null)
            data = data.Where(e => e.Price >= query.MinPrice && e.Price <= query.MaxPrice);

        var total = await data.CountAsync();

        var events = await data
            .Skip((query.Page - 1) * query.Limit)
            .Take(query.Limit)
            .ToListAsync();

        return Ok(new
        {
            events,
            total,
            page = query.Page,
            limit = query.Limit
        });
    }

    // ================= GET BY ID =================
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var ev = await _context.Events.FindAsync(id);

        if (ev == null)
            return NotFound("Event not found");

        return Ok(ev);
    }

    // ================= UPDATE =================
    [HttpPatch("update/{id}")]
    [Authorize(Roles = "Admin,Organizer")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateEventDto dto)
    {
        var ev = await _context.Events.FindAsync(id);

        if (ev == null)
            return NotFound("Event not found");

        ev.Title = dto.Title ?? ev.Title;
        ev.Description = dto.Description ?? ev.Description;
        ev.Location = dto.Location ?? ev.Location;
        ev.StartAt = dto.StartAt ?? ev.StartAt;
        ev.Price = dto.Price ?? ev.Price;
        ev.CategoryId = dto.CategoryId ?? ev.CategoryId;

        await _context.SaveChangesAsync();

        return Ok(ev);
    }

    // ================= DELETE =================
    [HttpDelete("delete/{id}")]
    [Authorize(Roles = "Admin,Organizer")]
    public async Task<IActionResult> Delete(int id)
    {
        var ev = await _context.Events.FindAsync(id);

        if (ev == null)
            return NotFound("Event not found");

        _context.Events.Remove(ev);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Deleted successfully" });
    }
}