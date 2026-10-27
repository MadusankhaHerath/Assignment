using Backend.Dtos;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Authorize]
[Route("api/locations")]
public class LocationsController(LocationService locations) : ControllerBase
{
    [HttpGet]
    public Task<List<LocationDto>> GetAll() => locations.GetAllAsync();
}
