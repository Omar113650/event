using Eventix_Project.DTOs.Booking;

namespace Eventix_Project.Services.Interfaces;

public interface IBookingService
{
    Task<BookingResponseDto> CreateAsync(CreateBookingDto dto, int userId);
    Task<List<BookingResponseDto>> GetUserBookingsAsync(int userId);
    Task<List<BookingResponseDto>> GetAllAsync();
    Task<bool> CancelAsync(int id, int userId);
}