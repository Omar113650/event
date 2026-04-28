using Eventix_Project.DTOs.Booking;
using Eventix_Project.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eventix_Project.Controllers;

[ApiController]
[Route("api/bookings")]
[Authorize]
public class BookingController : ControllerBase
{
    private readonly IBookingService _service;

    public BookingController(IBookingService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateBookingDto dto)
    {
        int userId = int.Parse(User.FindFirst("userId")!.Value);
        return Ok(await _service.CreateAsync(dto, userId));
    }

    [HttpGet("my")]
    public async Task<IActionResult> MyBookings()
    {
        int userId = int.Parse(User.FindFirst("userId")!.Value);
        return Ok(await _service.GetUserBookingsAsync(userId));
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
        => Ok(await _service.GetAllAsync());

    [HttpDelete("{id}")]
    public async Task<IActionResult> Cancel(int id)
    {
        int userId = int.Parse(User.FindFirst("userId")!.Value);
        return Ok(await _service.CancelAsync(id, userId));
    }
}