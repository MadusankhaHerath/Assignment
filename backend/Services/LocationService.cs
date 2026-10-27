using Backend.Data;
using Backend.Dtos;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class LocationService(AppDbContext db)
{
    public async Task SaveAsync(List<LocationDto> locations)
    {
        var existing = await db.LocationDetails.ToDictionaryAsync(l => l.Location_Code);

        foreach (var loc in locations.DistinctBy(l => l.LocationCode))
        {
            if (existing.TryGetValue(loc.LocationCode, out var row))
                row.Location_Name = loc.LocationName;
            else
                db.LocationDetails.Add(new LocationDetail { Location_Code = loc.LocationCode, Location_Name = loc.LocationName });
        }

        await db.SaveChangesAsync();
    }

    public Task<List<LocationDto>> GetAllAsync() =>
        db.LocationDetails
            .OrderBy(l => l.Location_Name)
            .Select(l => new LocationDto(l.Location_Code, l.Location_Name))
            .ToListAsync();
}
