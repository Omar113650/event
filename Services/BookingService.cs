using Eventix_Project.Data;
using Eventix_Project.DTOs.Booking;
using Eventix_Project.Models;
using Eventix_Project.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Eventix_Project.Services.Implementations;

public class BookingService : IBookingService
{
    private readonly AppDbContext _context;

    public BookingService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<BookingResponseDto> CreateAsync(CreateBookingDto dto, int userId)
    {
        var ev = await _context.Events.FindAsync(dto.EventId);
        if (ev == null) throw new Exception("Event not found");

        if (ev.Capacity < dto.Seats)
            throw new Exception("Not enough seats");

        var booking = new Booking
        {
            EventId = dto.EventId,
            TicketId = dto.TicketId,
            UserId = userId,
            Seats = dto.Seats,
            Status = "confirmed",
            TotalPrice = ev.Price * dto.Seats,
            CreatedAt = DateTime.UtcNow
        };

        ev.Capacity -= dto.Seats;

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        return new BookingResponseDto
        {
            Id = booking.Id,
            EventId = booking.EventId,
            Seats = booking.Seats,
            TotalPrice = booking.TotalPrice,
            Status = booking.Status,
            CreatedAt = booking.CreatedAt,
            UserId = booking.UserId
        };
    }

    public async Task<List<BookingResponseDto>> GetUserBookingsAsync(int userId)
    {
        return await _context.Bookings
            .Where(b => b.UserId == userId)
            .Include(b => b.Event)
            .Select(b => new BookingResponseDto
            {
                Id = b.Id,
                EventId = b.EventId,
                EventTitle = b.Event!.Title,
                Seats = b.Seats,
                TotalPrice = b.TotalPrice,
                Status = b.Status,
                CreatedAt = b.CreatedAt,
                UserId = b.UserId
            })
            .ToListAsync();
    }

    public async Task<List<BookingResponseDto>> GetAllAsync()
    {
        return await _context.Bookings
            .Include(b => b.Event)
            .Select(b => new BookingResponseDto
            {
                Id = b.Id,
                EventId = b.EventId,
                EventTitle = b.Event!.Title,
                Seats = b.Seats,
                TotalPrice = b.TotalPrice,
                Status = b.Status,
                CreatedAt = b.CreatedAt,
                UserId = b.UserId
            })
            .ToListAsync();
    }

    public async Task<bool> CancelAsync(int id, int userId)
    {
        var booking = await _context.Bookings
            .Include(b => b.Event)
            .FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);

        if (booking == null) return false;

        booking.Status = "cancelled";
        booking.Event!.Capacity += booking.Seats;

        await _context.SaveChangesAsync();
        return true;
    }
}