using Eventix_Project.DTOs.EventCategory;
using Eventix_Project.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eventix_Project.Controllers;

[ApiController]
[Route("api/categories")]
public class EventCategoryController : ControllerBase
{
    private readonly IEventCategoryService _service;

    public EventCategoryController(IEventCategoryService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result == null ? NotFound("Category not found") : Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateEventCategoryDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, UpdateEventCategoryDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        return result ? Ok("Updated") : NotFound("Category not found");
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        return result ? Ok("Deleted") : NotFound("Category not found");
    }
}